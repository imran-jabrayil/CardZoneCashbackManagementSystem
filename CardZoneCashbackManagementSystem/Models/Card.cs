using System.ComponentModel.DataAnnotations;

namespace CardZoneCashbackManagementSystem.Models;

public class Card
{
    public long Id { get; set; }
    public string Pan { get; set; } = null!;
    [MaxLength(100)]
    public string CustomerId { get; set; } = null!;
    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Transaction> Transactions { get; } = new List<Transaction>();
}