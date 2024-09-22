using Microsoft.EntityFrameworkCore;

using CardZoneCashbackManagementSystem.Models;


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
        modelBuilder.Entity<Card>()
            .HasMany(c => c.Transactions)
            .WithOne(t => t.Card)
            .HasForeignKey(t => t.CardId)
            .IsRequired();

        modelBuilder.Entity<Card>()
            .Property(c => c.Pan)
            .HasMaxLength(16);
        
        base.OnModelCreating(modelBuilder);
    }
}