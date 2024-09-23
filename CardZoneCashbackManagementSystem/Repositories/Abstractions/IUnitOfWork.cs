namespace CardZoneCashbackManagementSystem.Repositories.Abstractions;

public interface IUnitOfWork : IDisposable
{
    ICardRepository CardRepository { get; }
    ITransactionRepository TransactionRepository { get; }
    IJobStateRepository JobStateRepository { get; }
    Task SaveAsync();
}