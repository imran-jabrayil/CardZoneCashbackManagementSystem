namespace CardZoneCashbackManagementSystem.Models.Requests;

public class CreateCardRequest
{
    public string Pan { get; set; }
    public string CustomerId { get; set; }
}