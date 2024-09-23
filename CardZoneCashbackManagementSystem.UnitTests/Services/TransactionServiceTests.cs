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
        Mock<ITransactionRepository> transactionRepository,
        Mock<IUnitOfWork> unitOfWork)
    {
        transactionRepository.Setup(tr => tr.GetTransactionsAsync())
            .ReturnsAsync(transactions)
            .Verifiable();
        unitOfWork.Setup(uow => uow.TransactionRepository)
            .Returns(transactionRepository.Object)
            .Verifiable();

        var sut = new TransactionService(logger.Object, unitOfWork.Object,
            cashbackClient.Object);

        var actualTransactions = await sut.GetTransactionsAsync();

        transactionRepository.Verify(tr => tr.GetTransactionsAsync(), Times.Once());
        unitOfWork.Verify(uow => uow.TransactionRepository, Times.Once());
        unitOfWork.Verify(uow => uow.SaveAsync(), Times.Never());
        actualTransactions.Should().BeEquivalentTo(transactions);
    }

    [Theory]
    [InlineAutoData]
    public async Task GetTransactionByIdAsync_WhenCalled_IdExists_TransactionReturned(
        Transaction transaction,
        Mock<ILogger<TransactionService>> logger,
        Mock<ICashbackClient> cashbackClient,
        Mock<ITransactionRepository> transactionRepository,
        Mock<IUnitOfWork> unitOfWork)
    {
        transactionRepository.Setup(tr => tr.GetTransactionByIdAsync(transaction.Id))
            .ReturnsAsync(transaction)
            .Verifiable();
        unitOfWork.Setup(uow => uow.TransactionRepository)
            .Returns(transactionRepository.Object)
            .Verifiable();

        var sut = new TransactionService(logger.Object, unitOfWork.Object,
            cashbackClient.Object);

        var actualTransaction = await sut.GetTransactionByIdAsync(transaction.Id);

        transactionRepository.Verify(tr => tr.GetTransactionByIdAsync(transaction.Id), Times.Once());
        unitOfWork.Verify(uow => uow.TransactionRepository, Times.Once());
        unitOfWork.Verify(uow => uow.SaveAsync(), Times.Never());
        actualTransaction.Should().NotBeNull()
            .And.BeEquivalentTo(transaction);
    }

    [Theory]
    [InlineAutoData]
    public async Task AddTransactionAsync_WhenCalled_NothingReturned(
        Transaction transaction,
        Card card,
        Mock<ILogger<TransactionService>> logger,
        Mock<ICashbackClient> cashbackClient,
        Mock<ICardRepository> cardRepository,
        Mock<ITransactionRepository> transactionRepository,
        Mock<IUnitOfWork> unitOfWork)
    {
        unitOfWork.Setup(uow => uow.TransactionRepository)
            .Returns(transactionRepository.Object)
            .Verifiable();
        unitOfWork.Setup(uow => uow.CardRepository)
            .Returns(cardRepository.Object)
            .Verifiable();
        cardRepository.Setup(cr => cr.GetCardByIdAsync(transaction.CardId))
            .ReturnsAsync(card)
            .Verifiable();

        var sut = new TransactionService(logger.Object, unitOfWork.Object,
            cashbackClient.Object);

        await sut.AddTransactionAsync(transaction);

        transactionRepository.Verify(tr => tr.AddTransactionAsync(transaction), Times.Once());
        cardRepository.Verify(cr => cr.GetCardByIdAsync(transaction.CardId), Times.Once());
        unitOfWork.Verify(uow => uow.SaveAsync(), Times.Once());
        unitOfWork.Verify(uow => uow.TransactionRepository, Times.Once());
        unitOfWork.Verify(uow => uow.CardRepository, Times.Once());
    }

    [Theory]
    [AutoData]
    public async Task GetTransactionsAsync_WithValidFromDate_ReturnsFilteredTransactions(
        List<Transaction> transactions,
        Mock<ILogger<TransactionService>> logger,
        Mock<ICashbackClient> cashbackClient,
        Mock<ITransactionRepository> transactionRepository,
        Mock<IUnitOfWork> unitOfWork)
    {
        // Arrange
        var from = transactions.Max(tx => tx.CreatedAt);

        transactionRepository.Setup(r => r.GetTransactionsAsync(from))
            .ReturnsAsync(transactions.Where(t => t.CreatedAt >= from).ToList())
            .Verifiable();
        unitOfWork.Setup(uow => uow.TransactionRepository)
            .Returns(transactionRepository.Object)
            .Verifiable();

        var sut = new TransactionService(logger.Object, unitOfWork.Object,
            cashbackClient.Object);

        // Act
        var result = await sut.GetTransactionsAsync(from);

        // Assert
        result.Should().HaveCount(transactions.Count(t => t.CreatedAt >= from));
        transactionRepository.Verify(r => r.GetTransactionsAsync(from), Times.Once());
        unitOfWork.Verify(uow => uow.SaveAsync(), Times.Never());
        unitOfWork.Verify(uow => uow.TransactionRepository, Times.Once());
    }

    [Theory]
    [AutoData]
    public async Task GetTransactionsAsync_WithValidFromAndToDate_ReturnsFilteredTransactions(
        List<Transaction> transactions,
        Mock<ILogger<TransactionService>> logger,
        Mock<ICashbackClient> cashbackClient,
        Mock<ITransactionRepository> transactionRepository,
        Mock<IUnitOfWork> unitOfWork)
    {
        // Arrange
        var from = transactions.Max(tx => tx.CreatedAt);
        var to = from.AddSeconds(1);

        // Simulate the behavior of the repository
        transactionRepository.Setup(r => r.GetTransactionsAsync(from, to))
            .ReturnsAsync(transactions.Where(t => t.CreatedAt >= from && t.CreatedAt < to).ToList())
            .Verifiable();
        unitOfWork.Setup(uow => uow.TransactionRepository)
            .Returns(transactionRepository.Object)
            .Verifiable();

        var sut = new TransactionService(logger.Object, unitOfWork.Object,
            cashbackClient.Object);

        // Act
        var result = await sut.GetTransactionsAsync(from, to);

        // Assert
        result.Should().HaveCount(transactions.Count(t => t.CreatedAt >= from));
        transactionRepository.Verify(r => r.GetTransactionsAsync(from, to), Times.Once());
        unitOfWork.Verify(uow => uow.SaveAsync(), Times.Never());
        unitOfWork.Verify(uow => uow.TransactionRepository, Times.Once());
    }
}