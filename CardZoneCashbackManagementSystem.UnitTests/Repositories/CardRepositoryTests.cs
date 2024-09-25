using AutoFixture.Xunit2;
using CardZoneCashbackManagementSystem.Database;
using CardZoneCashbackManagementSystem.Models;
using CardZoneCashbackManagementSystem.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace CardZoneCashbackManagementSystem.UnitTests.Repositories;

public class CardRepositoryTests
{
    private readonly AppDbContext _dbContext;


    public CardRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new AppDbContext(options);
    }


    [Theory, InlineAutoData]
    public async Task GetCardsAsync_ReturnsAllCards(List<Card> cards)
    {
        // Arrange
        var repository = new CardRepository(_dbContext);

        await _dbContext.Cards.AddRangeAsync(cards);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await repository.GetCardsAsync();

        // Assert
        result.Should().HaveCount(cards.Count);
    }

    [Theory, InlineAutoData]
    public async Task GetCardByIdAsync_WhenCardExists_ReturnsCard(Card card)
    {
        // Arrange
        var repository = new CardRepository(_dbContext);

        await _dbContext.Cards.AddAsync(card);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await repository.GetCardByIdAsync(card.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(card.Id);
    }

    [Theory, InlineAutoData]
    public async Task AddCardAsync_AddsCard(Card card)
    {
        // Arrange
        var repository = new CardRepository(_dbContext);

        // Act
        await repository.AddCardAsync(card);

        // Assert
        var savedCard = await _dbContext.Cards.FindAsync(card.Id);
        savedCard.Should().NotBeNull();
        savedCard!.Pan.Should().Be(card.Pan);
        savedCard.Balance.Should().Be(card.Balance);
        savedCard.CustomerId.Should().Be(card.CustomerId);
    }

    [Theory, InlineAutoData]
    public async Task DeleteCardByIdAsync_WhenCardExists_DeletesCard(Card card)
    {
        // Arrange
        var repository = new CardRepository(_dbContext);

        await _dbContext.Cards.AddAsync(card);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await repository.DeleteCardByIdAsync(card.Id);
        await _dbContext.SaveChangesAsync();

        // Assert
        result.Should().BeTrue();
        var deletedCard = await _dbContext.Cards.FindAsync(card.Id);
        deletedCard.Should().BeNull();
    }

    [Theory, InlineAutoData]
    public async Task DeleteCardByIdAsync_WhenCardDoesNotExist_ReturnsFalse(long id)
    {
        // Arrange
        var repository = new CardRepository(_dbContext);

        // Act
        var result = await repository.DeleteCardByIdAsync(id);

        // Assert
        result.Should().BeFalse();
    }
}