using AutoMapper;
using CardZoneCashbackManagementSystem.Exceptions;
using CardZoneCashbackManagementSystem.Models;
using CardZoneCashbackManagementSystem.Models.Requests;
using CardZoneCashbackManagementSystem.Models.Validators;
using CardZoneCashbackManagementSystem.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace CardZoneCashbackManagementSystem.Controllers;

[ApiController]
[Route("/api/")]
public class TransactionController : ControllerBase
{
    private readonly ILogger<TransactionController> _logger;
    private readonly ITransactionService _transactionService;
    private readonly CreateTransactionRequestValidator _createTransactionRequestValidator;
    private readonly IMapper _mapper;

    public TransactionController(
        ILogger<TransactionController> logger,
        ITransactionService transactionService,
        CreateTransactionRequestValidator createTransactionRequestValidator,
        IMapper mapper)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
        _createTransactionRequestValidator = createTransactionRequestValidator ??
                                             throw new ArgumentNullException(nameof(createTransactionRequestValidator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }


    [HttpPost]
    [Route("cards/{cardId}/transactions")]
    public async Task<IActionResult> AddTransaction(long cardId, [FromBody] CreateTransactionRequest request)
    {
        var validationResult = await _createTransactionRequestValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var errorMessage = validationResult.Errors.First().ErrorMessage;
            _logger.LogError("invalid request: {ErrorMessage}", errorMessage);
            return BadRequest(errorMessage);
        }

        var transaction = _mapper.Map<Transaction>(request);
        transaction.CardId = cardId;

        try
        {
            await _transactionService.AddTransactionAsync(transaction);
        }
        catch (OverpaymentException ex)
        {
            _logger.LogError("invalid request: {ErrorMessage}", ex.Message);
            return UnprocessableEntity("Balance is not enough");
        }
        
        _logger.LogInformation("transaction added with id {Id}", transaction.Id);
        return Ok();
    }
}