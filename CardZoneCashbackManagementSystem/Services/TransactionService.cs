using CardZoneCashbackManagementSystem.Clients.Abstractions;
using CardZoneCashbackManagementSystem.Constants;
using CardZoneCashbackManagementSystem.Models;
using CardZoneCashbackManagementSystem.Repositories.Abstractions;
using CardZoneCashbackManagementSystem.Services.Abstractions;

namespace CardZoneCashbackManagementSystem.Services;

public class TransactionService : ITransactionService
{
    private readonly ILogger<TransactionService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICashbackClient _cashbackClient;


    public TransactionService(
        ILogger<TransactionService> logger,
        IUnitOfWork unitOfWork,
        ICashbackClient cashbackClient)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _cashbackClient = cashbackClient ?? throw new ArgumentNullException(nameof(cashbackClient));
    }

    public async Task<ICollection<Transaction>> GetTransactionsAsync()
    {
        return await _unitOfWork.TransactionRepository.GetTransactionsAsync();
    }

    public async Task<ICollection<Transaction>> GetTransactionsAsync(DateTime from)
    {
        return await _unitOfWork.TransactionRepository.GetTransactionsAsync(from);
    }

    public async Task<ICollection<Transaction>> GetTransactionsAsync(DateTime from, DateTime to)
    {
        return await _unitOfWork.TransactionRepository.GetTransactionsAsync(from, to);
    }

    public async Task<Transaction?> GetTransactionByIdAsync(long id)
    {
        return await _unitOfWork.TransactionRepository.GetTransactionByIdAsync(id);
    }

    public async Task AddTransactionAsync(Transaction transaction)
    {
        var card = await _unitOfWork.CardRepository.GetCardByIdAsync(transaction.CardId);
        if (card is null)
        {
            _logger.LogError("card with id {Id} not found", transaction.CardId);
            return;
        }

        var changedAmount = transaction.Type == TransactionTypes.Credit ? transaction.Amount : -transaction.Amount;
        card.Balance += changedAmount;

        await _unitOfWork.TransactionRepository.AddTransactionAsync(transaction);
        await _unitOfWork.SaveAsync();
    }

    public async Task<bool> DeleteTransactionByIdAsync(long id)
    {
        var result = await _unitOfWork.TransactionRepository.DeleteTransactionByIdAsync(id);
        await _unitOfWork.SaveAsync();
        return result;
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
            await _unitOfWork.TransactionRepository.AddTransactionAsync(new Transaction
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