namespace CardZoneCashbackManagementSystem.Repositories.Abstractions;

public interface IUnitOfWork : IDisposable
{
    ICardRepository CardRepository { get; }
    ITransactionRepository TransactionRepository { get; }
    Task SaveAsync();
}