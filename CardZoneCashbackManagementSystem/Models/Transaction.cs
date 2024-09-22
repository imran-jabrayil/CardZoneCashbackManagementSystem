using System.ComponentModel.DataAnnotations;

namespace CardZoneCashbackManagementSystem.Models;

public class Transaction
{
    public long Id { get; set; }
    [MaxLength(16)]
    public string Type { get; set; } = null!;
    public decimal Amount { get; set; }
    public long CardId { get; set; }
    public bool HasCashback { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public Card Card { get; set; } = null!;
}