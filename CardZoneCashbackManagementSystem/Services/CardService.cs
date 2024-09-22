using CardZoneCashbackManagementSystem.Models;
using CardZoneCashbackManagementSystem.Repositories.Abstractions;
using CardZoneCashbackManagementSystem.Services.Abstractions;

namespace CardZoneCashbackManagementSystem.Services;

public class CardService : ICardService
{
    private readonly ICardRepository _cardRepository;

    
    public CardService(ICardRepository cardRepository)
    {
        _cardRepository = cardRepository;
    }

    
    public async Task<ICollection<Card>> GetCardsAsync()
    {
        return await _cardRepository.GetCardsAsync();
    }

    public async Task<Card?> GetCardByIdAsync(long id)
    {
        return await _cardRepository.GetCardByIdAsync(id);
    }

    public async Task AddCardAsync(Card card)
    {
        await _cardRepository.AddCardAsync(card);
    }

    public async Task<bool> DeleteCardByIdAsync(long id)
    {
        return await _cardRepository.DeleteCardByIdAsync(id);
    }
}