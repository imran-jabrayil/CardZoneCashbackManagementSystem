using AutoMapper;
using CardZoneCashbackManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

using CardZoneCashbackManagementSystem.Models.Requests;
using CardZoneCashbackManagementSystem.Models.Validators;
using CardZoneCashbackManagementSystem.Repositories.Abstractions;
using CardZoneCashbackManagementSystem.Services.Abstractions;


namespace CardZoneCashbackManagementSystem.Controllers;

[ApiController]
[Route("/api/")]
public class CardController : ControllerBase
{
    private readonly ICardService _cardService;
    private readonly CreateCardRequestValidator _createCardRequestValidator;
    private readonly IMapper _mapper;
    
    
    public CardController(
        ICardService cardService, 
        CreateCardRequestValidator createCardRequestValidator, 
        IMapper mapper)
    {
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
        return Ok(cards);
    }

    [HttpGet]
    [Route("cards/{id}")]
    public async Task<IActionResult> GetCards(long id)
    {
        var card = await _cardService.GetCardByIdAsync(id);

        return card is not null
            ? Ok(card)
            : NotFound();
    }

    [HttpPost]
    [Route("cards")]
    public async Task<IActionResult> CreateCard([FromBody] CreateCardRequest request)
    {
        var validationResult = await _createCardRequestValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors.First().ErrorMessage);

        var card = _mapper.Map<Card>(request);

        await _cardService.AddCardAsync(card);
        return Ok();
    }

    [HttpDelete]
    [Route("cards/{id}")]
    public async Task<IActionResult> DeleteCard(long id)
    {
        return await _cardService.DeleteCardByIdAsync(id)
            ? Ok()
            : NotFound();
    }
}
