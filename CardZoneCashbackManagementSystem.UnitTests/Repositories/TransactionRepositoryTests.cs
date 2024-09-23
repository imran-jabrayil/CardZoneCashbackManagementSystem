using AutoFixture.Xunit2;
using CardZoneCashbackManagementSystem.Database;
using CardZoneCashbackManagementSystem.Models;
using CardZoneCashbackManagementSystem.Repositories;
using CardZoneCashbackManagementSystem.Repositories.Abstractions;
using CardZoneCashbackManagementSystem.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CardZoneCashbackManagementSystem.UnitTests.Repositories;

public class TransactionRepositoryTests
{
    private readonly AppDbContext _dbContext;


    public TransactionRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new AppDbContext(options);
    }


    [Theory]
    [AutoData]
    public async Task GetTransactionsAsync_ReturnsAllTransactions(List<Transaction> transactions)
    {
        // Arrange
        var repository = new TransactionRepository(_dbContext);

        await _dbContext.Transactions.AddRangeAsync(transactions);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await repository.GetTransactionsAsync();

        // Assert
        result.Should().HaveCount(transactions.Count);
    }

    [Theory]
    [AutoData]
    public async Task GetTransactionsAsync_ReturnsTransactions(List<Transaction> transactions)
    {
        // Arrange
        var from = transactions.Max(tx => tx.CreatedAt);
        var repository = new TransactionRepository(_dbContext);

        await _dbContext.Transactions.AddRangeAsync(transactions);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await repository.GetTransactionsAsync(from);

        // Assert
        result.Should().HaveCount(1);
    }

    [Theory]
    [AutoData]
    public async Task GetTransactionsAsync_WithValidFromDate_ReturnsFilteredTransactions(
        List<Transaction> transactions,
        Mock<ITransactionRepository> repository)
    {
        // Arrange
        var from = transactions.Max(tx => tx.CreatedAt);

        // Simulate the behavior of the repository
        repository.Setup(r => r.GetTransactionsAsync(from))
            .ReturnsAsync(transactions.Where(t => t.CreatedAt >= from).ToList())
            .Verifiable();

        var sut = new TransactionService(repository.Object);

        // Act
        var result = await sut.GetTransactionsAsync(from);

        // Assert
        result.Should().HaveCount(transactions.Count(t => t.CreatedAt >= from));
        repository.Verify(r => r.GetTransactionsAsync(from), Times.Once());
    }

    [Theory]
    [AutoData]
    public async Task GetTransactionByIdAsync_WhenTransactionExists_ReturnsCard(Transaction transaction)
    {
        // Arrange
        var repository = new TransactionRepository(_dbContext);

        await _dbContext.Transactions.AddAsync(transaction);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await repository.GetTransactionByIdAsync(transaction.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(transaction.Id);
    }

    [Theory]
    [AutoData]
    public async Task AddTransactionAsync_AddsTransaction(Transaction transaction)
    {
        // Arrange
        var repository = new TransactionRepository(_dbContext);

        // Act
        await repository.AddTransactionAsync(transaction);

        // Assert
        var savedTransaction = await _dbContext.Transactions.FindAsync(transaction.Id);
        savedTransaction.Should().NotBeNull();
        savedTransaction!.CardId.Should().Be(transaction.CardId);
        savedTransaction.HasCashback.Should().Be(transaction.HasCashback);
        savedTransaction.Amount.Should().Be(transaction.Amount);
        savedTransaction.Type.Should().Be(transaction.Type);
    }

    [Theory]
    [AutoData]
    public async Task DeleteTransactionByIdAsync_WhenTransactionExists_DeletesCard(Transaction transaction)
    {
        // Arrange
        var repository = new TransactionRepository(_dbContext);

        await _dbContext.Transactions.AddAsync(transaction);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await repository.DeleteTransactionByIdAsync(transaction.Id);

        // Assert
        result.Should().BeTrue();
        var deletedTransaction = await _dbContext.Transactions.FindAsync(transaction.Id);
        deletedTransaction.Should().BeNull();
    }

    [Theory]
    [AutoData]
    public async Task DeleteTransactionByIdAsync_WhenTransactionDoesNotExist_ReturnsFalse(long id)
    {
        // Arrange
        var repository = new TransactionRepository(_dbContext);

        // Act
        var result = await repository.DeleteTransactionByIdAsync(id);

        // Assert
        result.Should().BeFalse();
    }
}