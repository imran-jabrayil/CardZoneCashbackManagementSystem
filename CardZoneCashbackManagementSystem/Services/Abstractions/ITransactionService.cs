using CardZoneCashbackManagementSystem.Models;

namespace CardZoneCashbackManagementSystem.Services.Abstractions;

public interface ITransactionService
{
    Task<ICollection<Transaction>> GetTransactionsAsync();
    Task<ICollection<Transaction>> GetTransactionsAsync(DateTime from);
    Task<ICollection<Transaction>> GetTransactionsAsync(DateTime from, DateTime to);
    Task<Transaction?> GetTransactionByIdAsync(long id);
    Task AddTransactionAsync(Transaction transaction);
    Task<decimal?> CalculateCashback(Transaction transaction, bool shouldCreditAccount = false);
}