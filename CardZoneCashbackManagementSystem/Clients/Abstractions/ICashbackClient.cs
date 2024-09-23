namespace CardZoneCashbackManagementSystem.Clients.Abstractions;

public interface ICashbackClient
{
    Task<decimal?> GetCashbackAmountAsync(decimal transactionAmount);
}