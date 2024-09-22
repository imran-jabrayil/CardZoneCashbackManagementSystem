using CardZoneCashbackManagementSystem.Models;

namespace CardZoneCashbackManagementSystem.Services.Abstractions;

public interface ICardService
{
    Task<ICollection<Card>> GetCardsAsync();
    Task<Card?> GetCardByIdAsync(long id);
    Task AddCardAsync(Card card);
    Task<bool> DeleteCardByIdAsync(long id);
}