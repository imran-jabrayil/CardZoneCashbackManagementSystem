namespace CardZoneCashbackManagementSystem.Models.Requests;

public class CreateCardRequest
{
    public string Pan { get; set; } = null!;
    public string CustomerId { get; set; } = null!;
}