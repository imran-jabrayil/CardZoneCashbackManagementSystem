using System.Diagnostics;
using System.Globalization;
using CardZoneCashbackManagementSystem.Services.Abstractions;
using Quartz;

namespace CardZoneCashbackManagementSystem.Jobs;

public class CashbackApplierJob : IJob
{
    private readonly ILogger<CashbackApplierJob> _logger;
    private readonly ITransactionService _transactionService;


    public CashbackApplierJob(
        ILogger<CashbackApplierJob> logger,
        ITransactionService transactionService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var sw = Stopwatch.StartNew();
        _logger.LogInformation("Started execution {Time}", DateTime.Now.ToString(CultureInfo.InvariantCulture));

        var transactions = await _transactionService.GetTransactionsAsync(DateTime.Today.AddDays(-1), DateTime.Today);

        foreach (var transaction in transactions)
        {
            var cashbackAmount =
                await _transactionService.CalculateCashback(transaction, true);

            if (cashbackAmount.HasValue)
                _logger.LogInformation("Credited card with id {CardId} with cashback amount {CashbackAmount}",
                    transaction.CardId, cashbackAmount.Value);
        }

        sw.Stop();
        _logger.LogInformation("Finished execution. Elapsed ms: {ElapsedMs}", sw.Elapsed.TotalMilliseconds);
    }
}