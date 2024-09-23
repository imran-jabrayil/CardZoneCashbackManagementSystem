using CardZoneCashbackManagementSystem.Models;
using CardZoneCashbackManagementSystem.Repositories.Abstractions;
using CardZoneCashbackManagementSystem.Services.Abstractions;

namespace CardZoneCashbackManagementSystem.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;


    public TransactionService(ITransactionRepository transactionRepository)
    {
        _transactionRepository =
            transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
    }

    public async Task<ICollection<Transaction>> GetTransactionsAsync()
    {
        return await _transactionRepository.GetTransactionsAsync();
    }

    public async Task<ICollection<Transaction>> GetTransactionsAsync(DateTime from)
    {
        return await _transactionRepository.GetTransactionsAsync(from);
    }

    public async Task<Transaction?> GetTransactionByIdAsync(long id)
    {
        return await _transactionRepository.GetTransactionByIdAsync(id);
    }

    public async Task AddTransactionAsync(Transaction transaction)
    {
        await _transactionRepository.AddTransactionAsync(transaction);
    }

    public async Task<bool> DeleteTransactionByIdAsync(long id)
    {
        return await _transactionRepository.DeleteTransactionByIdAsync(id);
    }
}