using CardZoneCashbackManagementSystem.Database;
using CardZoneCashbackManagementSystem.Models;
using CardZoneCashbackManagementSystem.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CardZoneCashbackManagementSystem.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _dbContext;

    public TransactionRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<ICollection<Transaction>> GetTransactionsAsync()
    {
        return await _dbContext.Transactions.ToListAsync();
    }

    public async Task<ICollection<Transaction>> GetTransactionsAsync(DateTime from)
    {
        return await _dbContext.Transactions
            .Where(tx => tx.CreatedAt >= from)
            .ToListAsync();
    }

    public async Task<ICollection<Transaction>> GetTransactionsAsync(DateTime from, DateTime to)
    {
        return await _dbContext.Transactions
            .Where(tx => tx.CreatedAt >= from && tx.CreatedAt < to)
            .ToListAsync();
    }

    public async Task<Transaction?> GetTransactionByIdAsync(long id)
    {
        return await _dbContext.Transactions.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddTransactionAsync(Transaction transaction)
    {
        await _dbContext.Transactions.AddAsync(transaction);
    }

    public async Task<bool> DeleteTransactionByIdAsync(long id)
    {
        var transaction = await _dbContext.Transactions.FirstOrDefaultAsync(c => c.Id == id);
        if (transaction is null) return false;

        _dbContext.Transactions.Remove(transaction);
        return true;
    }
}