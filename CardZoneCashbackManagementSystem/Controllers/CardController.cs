using AutoMapper;
using CardZoneCashbackManagementSystem.Models;
using CardZoneCashbackManagementSystem.Models.Requests;
using CardZoneCashbackManagementSystem.Models.Validators;
using CardZoneCashbackManagementSystem.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace CardZoneCashbackManagementSystem.Controllers;

[ApiController]
[Route("/api/")]
public class CardController : ControllerBase
{
    private readonly ILogger<CardController> _logger;
    private readonly ICardService _cardService;
    private readonly CreateCardRequestValidator _createCardRequestValidator;
    private readonly IMapper _mapper;


    public CardController(
        ILogger<CardController> logger,
        ICardService cardService,
        CreateCardRequestValidator createCardRequestValidator,
        IMapper mapper)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _cardService = cardService ?? throw new ArgumentNullException(nameof(cardService));
        _createCardRequestValidator = createCardRequestValidator ??
                                      throw new ArgumentNullException(nameof(createCardRequestValidator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }


    [HttpGet]
    [Route("cards")]
    public async Task<IActionResult> GetCards()
    {
        var cards = await _cardService.GetCardsAsync();
        _logger.LogInformation("cards retrieved with count {Count}", cards.Count);
        return Ok(cards);
    }

    [HttpGet]
    [Route("cards/{id}")]
    public async Task<IActionResult> GetCards([FromRoute] long id)
    {
        var card = await _cardService.GetCardByIdAsync(id);

        if (card is null)
        {
            _logger.LogInformation("card with id {Id} not found", id);
            return NotFound();
        }

        _logger.LogInformation("card retrieved with id {Id}", card.Id);
        return Ok();
    }

    [HttpPost]
    [Route("cards")]
    public async Task<IActionResult> CreateCard([FromBody] CreateCardRequest request)
    {
        var validationResult = await _createCardRequestValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var errorMessage = validationResult.Errors.First().ErrorMessage;
            _logger.LogError("invalid request: {ErrorMessage}", errorMessage);
            return BadRequest(errorMessage);
        }

        var card = _mapper.Map<Card>(request);

        await _cardService.AddCardAsync(card);
        _logger.LogInformation("card added with id {Id}", card.Id);
        return Ok();
    }

    [HttpDelete]
    [Route("cards/{id}")]
    public async Task<IActionResult> DeleteCard([FromRoute] long id)
    {
        var result = await _cardService.DeleteCardByIdAsync(id);

        if (result is false)
        {
            _logger.LogInformation("card with id {Id} not found for deletion", id);
            return NotFound();
        }

        _logger.LogInformation("deleted card with id {Id}", id);
        return Ok();
    }
}