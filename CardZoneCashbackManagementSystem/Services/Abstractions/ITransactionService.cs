using CardZoneCashbackManagementSystem.Models;

namespace CardZoneCashbackManagementSystem.Services.Abstractions;

public interface ITransactionService
{
    Task<ICollection<Transaction>> GetTransactionsAsync();
    Task<ICollection<Transaction>> GetTransactionsAsync(DateTime from);
    Task<Transaction?> GetTransactionByIdAsync(long id);
    Task AddTransactionAsync(Transaction transaction);
    Task<bool> DeleteTransactionByIdAsync(long id);
}