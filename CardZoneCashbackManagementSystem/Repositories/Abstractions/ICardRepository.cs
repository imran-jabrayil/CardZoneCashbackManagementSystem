using CardZoneCashbackManagementSystem.Models;

namespace CardZoneCashbackManagementSystem.Repositories.Abstractions;

public interface ICardRepository
{
    Task<ICollection<Card>> GetCardsAsync();
    Task<Card?> GetCardByIdAsync(long id);
    Task AddCardAsync(Card card);
    Task<bool> DeleteCardByIdAsync(long id);
}