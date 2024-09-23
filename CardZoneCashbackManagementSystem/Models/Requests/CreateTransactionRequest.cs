namespace CardZoneCashbackManagementSystem.Models.Requests;

public class CreateTransactionRequest
{
    public decimal Amount { get; set; }
    public string Type { get; set; } = null!;
    public bool HasCashback { get; set; }
}