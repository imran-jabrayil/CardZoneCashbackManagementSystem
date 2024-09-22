using CardZoneCashbackManagementSystem.Database;
using CardZoneCashbackManagementSystem.Models;
using CardZoneCashbackManagementSystem.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;


namespace CardZoneCashbackManagementSystem.Repositories;

public class CardRepository : ICardRepository
{
    private readonly AppDbContext _dbContext;
    
    public CardRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }


    public async Task<ICollection<Card>> GetCardsAsync()
    {
        return await _dbContext.Cards.ToListAsync();
    }

    public async Task<Card?> GetCardByIdAsync(long id)
    {
        return await _dbContext.Cards.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddCardAsync(Card card)
    {
        await _dbContext.Cards.AddAsync(card);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> DeleteCardByIdAsync(long id)
    {
        var card = await _dbContext.Cards.FirstOrDefaultAsync(c => c.Id == id);
        if (card is null) return false;

        _dbContext.Cards.Remove(card);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}