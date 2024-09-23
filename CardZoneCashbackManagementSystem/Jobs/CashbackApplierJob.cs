using System.Diagnostics;
using CardZoneCashbackManagementSystem.Services.Abstractions;
using Quartz;

namespace CardZoneCashbackManagementSystem.Jobs;

public class CashbackApplierJob : IJob
{
    private readonly ILogger<CashbackApplierJob> _logger;
    private readonly ITransactionService _transactionService;
    private readonly IJobStateService _jobStateService;


    public CashbackApplierJob(
        ILogger<CashbackApplierJob> logger,
        ITransactionService transactionService,
        IJobStateService jobStateService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
        _jobStateService = jobStateService ?? throw new ArgumentNullException(nameof(jobStateService));
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var sw = Stopwatch.StartNew();
        _logger.LogInformation("started execution");

        var jobState = await _jobStateService.GetJobState(nameof(CashbackApplierJob));
        var today = DateTime.Today;
        var startDay = jobState?.LastExecutionDay.AddDays(1) ?? today;

        for (var day = startDay; day <= today; day = day.AddDays(1)) await processTransactions(day);

        await _jobStateService.UpdateJobState(nameof(CashbackApplierJob), today);

        sw.Stop();
        _logger.LogInformation("Finished execution. Elapsed ms: {ElapsedMs}", sw.Elapsed.TotalMilliseconds);
    }


    private async Task processTransactions(DateTime day)
    {
        _logger.LogInformation("started processing day {Day}", day.ToShortDateString());

        var transactions = await _transactionService.GetTransactionsAsync(day.AddDays(-1), day);

        foreach (var transaction in transactions)
        {
            if (transaction.HasCashback is false) continue;

            var cashbackAmount =
                await _transactionService.CalculateCashback(transaction, true);

            if (cashbackAmount.HasValue)
                _logger.LogInformation("Credited card with id {CardId} with cashback amount {CashbackAmount}",
                    transaction.CardId, cashbackAmount.Value);
        }

        _logger.LogInformation("finished processing day {Day}", day.ToShortDateString());
    }
}