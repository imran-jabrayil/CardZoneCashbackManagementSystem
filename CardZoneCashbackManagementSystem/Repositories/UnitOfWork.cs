using CardZoneCashbackManagementSystem.Database;
using CardZoneCashbackManagementSystem.Repositories.Abstractions;

namespace CardZoneCashbackManagementSystem.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;


    public UnitOfWork(
        AppDbContext dbContext,
        ICardRepository cardRepository,
        ITransactionRepository transactionRepository,
        IJobStateRepository jobStateRepository)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        CardRepository = cardRepository ?? throw new ArgumentNullException(nameof(cardRepository));
        TransactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
        JobStateRepository = jobStateRepository ?? throw new ArgumentNullException(nameof(jobStateRepository));
    }

    public ICardRepository CardRepository { get; }
    public ITransactionRepository TransactionRepository { get; }
    public IJobStateRepository JobStateRepository { get; }


    public void Dispose()
    {
        _dbContext.Dispose();
    }


    public async Task SaveAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}