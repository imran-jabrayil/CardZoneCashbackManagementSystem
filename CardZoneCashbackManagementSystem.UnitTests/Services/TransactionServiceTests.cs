using AutoFixture.Xunit2;
using CardZoneCashbackManagementSystem.Clients.Abstractions;
using CardZoneCashbackManagementSystem.Models;
using CardZoneCashbackManagementSystem.Repositories.Abstractions;
using CardZoneCashbackManagementSystem.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace CardZoneCashbackManagementSystem.UnitTests.Services;

public class TransactionServiceTests
{
    [Theory]
    [InlineAutoData]
    public async Task GetTransactionsAsync_WhenCalled_TransactionsReturned(
        ICollection<Transaction> transactions,
        Mock<ILogger<TransactionService>> logger,
        Mock<ICashbackClient> cashbackClient,
        Mock<ITransactionRepository> transactionRepository)
    {
        transactionRepository.Setup(tr => tr.GetTransactionsAsync())
            .ReturnsAsync(transactions)
            .Verifiable();

        var sut = new TransactionService(logger.Object,
            transactionRepository.Object,
            cashbackClient.Object);

        var actualTransactions = await sut.GetTransactionsAsync();

        transactionRepository.Verify(tr => tr.GetTransactionsAsync(),
            Times.Once());
        actualTransactions.Should().BeEquivalentTo(transactions);
    }

    [Theory]
    [InlineAutoData]
    public async Task GetTransactionByIdAsync_WhenCalled_IdExists_TransactionReturned(
        Transaction transaction,
        Mock<ILogger<TransactionService>> logger,
        Mock<ICashbackClient> cashbackClient,
        Mock<ITransactionRepository> transactionRepository)
    {
        transactionRepository.Setup(tr => tr.GetTransactionByIdAsync(transaction.Id))
            .ReturnsAsync(transaction)
            .Verifiable();

        var sut = new TransactionService(logger.Object,
            transactionRepository.Object,
            cashbackClient.Object);

        var actualTransaction = await sut.GetTransactionByIdAsync(transaction.Id);

        transactionRepository.Verify(tr => tr.GetTransactionByIdAsync(transaction.Id),
            Times.Once());
        actualTransaction.Should().NotBeNull()
            .And.BeEquivalentTo(transaction);
    }

    [Theory]
    [InlineAutoData]
    public async Task AddTransactionAsync_WhenCalled_NothingReturned(
        Transaction transaction,
        Mock<ILogger<TransactionService>> logger,
        Mock<ICashbackClient> cashbackClient,
        Mock<ITransactionRepository> transactionRepository)
    {
        var sut = new TransactionService(logger.Object,
            transactionRepository.Object,
            cashbackClient.Object);

        await sut.AddTransactionAsync(transaction);

        transactionRepository.Verify(tr => tr.AddTransactionAsync(transaction),
            Times.Once());
    }

    [Theory]
    [InlineAutoData(true)]
    [InlineAutoData(false)]
    public async Task DeleteTransactionByIdAsync_WhenCalled_SuccessfulRemovalReturned(
        bool successfullyRemoved,
        Transaction transaction,
        Mock<ILogger<TransactionService>> logger,
        Mock<ICashbackClient> cashbackClient,
        Mock<ITransactionRepository> transactionRepository)
    {
        transactionRepository.Setup(tr => tr.DeleteTransactionByIdAsync(transaction.Id))
            .ReturnsAsync(successfullyRemoved)
            .Verifiable();

        var sut = new TransactionService(logger.Object,
            transactionRepository.Object,
            cashbackClient.Object);

        var actualRemoval = await sut.DeleteTransactionByIdAsync(transaction.Id);

        transactionRepository.Verify(tr => tr.DeleteTransactionByIdAsync(transaction.Id),
            Times.Once());
        actualRemoval.Should().Be(successfullyRemoved);
    }

    [Theory]
    [AutoData]
    public async Task GetTransactionsAsync_WithValidFromDate_ReturnsFilteredTransactions(
        List<Transaction> transactions,
        Mock<ILogger<TransactionService>> logger,
        Mock<ICashbackClient> cashbackClient,
        Mock<ITransactionRepository> transactionRepository)
    {
        // Arrange
        var from = transactions.Max(tx => tx.CreatedAt);

        // Simulate the behavior of the repository
        transactionRepository.Setup(r => r.GetTransactionsAsync(from))
            .ReturnsAsync(transactions.Where(t => t.CreatedAt >= from).ToList())
            .Verifiable();

        var sut = new TransactionService(logger.Object,
            transactionRepository.Object,
            cashbackClient.Object);

        // Act
        var result = await sut.GetTransactionsAsync(from);

        // Assert
        result.Should().HaveCount(transactions.Count(t => t.CreatedAt >= from));
        transactionRepository.Verify(r => r.GetTransactionsAsync(from), Times.Once());
    }
}