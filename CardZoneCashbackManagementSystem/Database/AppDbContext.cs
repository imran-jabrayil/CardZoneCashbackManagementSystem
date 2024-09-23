using CardZoneCashbackManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CardZoneCashbackManagementSystem.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }


    public DbSet<Card> Cards { get; set; }
    public DbSet<Transaction> Transactions { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Card>(card =>
        {
            card.HasMany(c => c.Transactions)
                .WithOne(t => t.Card)
                .HasForeignKey(t => t.CardId)
                .IsRequired();

            card.Property(c => c.Balance)
                .HasPrecision(10, 2);
            
            card.Property(c => c.Pan)
                .HasMaxLength(16);
        });

        modelBuilder.Entity<Transaction>(transaction =>
        {
            transaction.Property(t => t.Amount)
                .HasPrecision(10, 2);
        });

        base.OnModelCreating(modelBuilder);
    }
}