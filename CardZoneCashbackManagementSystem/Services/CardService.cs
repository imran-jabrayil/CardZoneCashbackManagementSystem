using CardZoneCashbackManagementSystem.Models;
using CardZoneCashbackManagementSystem.Repositories.Abstractions;
using CardZoneCashbackManagementSystem.Services.Abstractions;

namespace CardZoneCashbackManagementSystem.Services;

public class CardService : ICardService
{
    private readonly IUnitOfWork _unitOfWork;

    
    public CardService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    
    public async Task<ICollection<Card>> GetCardsAsync()
    {
        return await _unitOfWork.CardRepository.GetCardsAsync();
    }

    public async Task<Card?> GetCardByIdAsync(long id)
    {
        return await _unitOfWork.CardRepository.GetCardByIdAsync(id);
    }

    public async Task AddCardAsync(Card card)
    {
        await _unitOfWork.CardRepository.AddCardAsync(card);
        await _unitOfWork.SaveAsync();
    }

    public async Task<bool> DeleteCardByIdAsync(long id)
    {
        var result = await _unitOfWork.CardRepository.DeleteCardByIdAsync(id);
        await _unitOfWork.SaveAsync();
        return result;
    }
}