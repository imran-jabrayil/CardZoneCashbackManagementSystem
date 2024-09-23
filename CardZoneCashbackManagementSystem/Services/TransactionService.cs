using CardZoneCashbackManagementSystem.Clients.Abstractions;
using CardZoneCashbackManagementSystem.Constants;
using CardZoneCashbackManagementSystem.Models;
using CardZoneCashbackManagementSystem.Repositories.Abstractions;
using CardZoneCashbackManagementSystem.Services.Abstractions;

namespace CardZoneCashbackManagementSystem.Services;

public class TransactionService : ITransactionService
{
    private readonly ILogger<TransactionService> _logger;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICashbackClient _cashbackClient;


    public TransactionService(
        ILogger<TransactionService> logger,
        ITransactionRepository transactionRepository,
        ICashbackClient cashbackClient)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _transactionRepository =
            transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
        _cashbackClient = cashbackClient ?? throw new ArgumentNullException(nameof(cashbackClient));
    }

    public async Task<ICollection<Transaction>> GetTransactionsAsync()
    {
        return await _transactionRepository.GetTransactionsAsync();
    }

    public async Task<ICollection<Transaction>> GetTransactionsAsync(DateTime from)
    {
        return await _transactionRepository.GetTransactionsAsync(from);
    }

    public async Task<ICollection<Transaction>> GetTransactionsAsync(DateTime from, DateTime to)
    {
        return await _transactionRepository.GetTransactionsAsync(from, to);
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

    public async Task<decimal?> CalculateCashback(Transaction transaction, bool shouldCreditAccount = false)
    {
        var cashbackAmount = await _cashbackClient.GetCashbackAmountAsync(transaction.Amount);
        if (cashbackAmount is null)
        {
            _logger.LogWarning("Could not receive cashback amount for transaction with id {Id}", transaction.Id);
            return cashbackAmount;
        }

        if (shouldCreditAccount)
            await _transactionRepository.AddTransactionAsync(new Transaction
            {
                Amount = cashbackAmount.Value,
                CardId = transaction.CardId,
                CreatedAt = DateTime.Now,
                HasCashback = false,
                Type = TransactionTypes.Credit
            });

        return cashbackAmount;
    }
}