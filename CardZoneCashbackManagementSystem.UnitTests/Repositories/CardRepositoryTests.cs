using AutoFixture.Xunit2;
using FluentAssertions;

using CardZoneCashbackManagementSystem.Models;
using CardZoneCashbackManagementSystem.Repositories;
using CardZoneCashbackManagementSystem.UnitTests.Fixtures;

namespace CardZoneCashbackManagementSystem.UnitTests.Repositories;

public class CardRepositoryTests : IClassFixture<DatabaseFixture>, IDisposable
{
    private readonly DatabaseFixture _fixture;

    
    public CardRepositoryTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }
    
    
    public void Dispose()
    {
        _fixture.Dispose();
    }

    
    [Theory, AutoData]
    public async Task GetCardsAsync_ReturnsAllCards(List<Card> cards)
    {
        // Arrange
        await using var context = _fixture.CreateContext();
        var repository = new CardRepository(context);

        await context.Cards.AddRangeAsync(cards);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetCardsAsync();

        // Assert
        result.Should().HaveCount(cards.Count);
    }

    [Theory, AutoData]
    public async Task GetCardByIdAsync_WhenCardExists_ReturnsCard(Card card)
    {
        // Arrange
        await using var context = _fixture.CreateContext();
        var repository = new CardRepository(context);

        await context.Cards.AddAsync(card);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetCardByIdAsync(card.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(card.Id);
    }

    [Theory, AutoData]
    public async Task AddCardAsync_AddsCard(Card card)
    {
        // Arrange
        await using var context = _fixture.CreateContext();
        var repository = new CardRepository(context);

        // Act
        await repository.AddCardAsync(card);

        // Assert
        var savedCard = await context.Cards.FindAsync(card.Id);
        savedCard.Should().NotBeNull();
        savedCard!.Pan.Should().Be(card.Pan);
    }

    [Theory, AutoData]
    public async Task DeleteCardByIdAsync_WhenCardExists_DeletesCard(Card card)
    {
        // Arrange
        await using var context = _fixture.CreateContext();
        var repository = new CardRepository(context);

        await context.Cards.AddAsync(card);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.DeleteCardByIdAsync(card.Id);

        // Assert
        result.Should().BeTrue();
        var deletedCard = await context.Cards.FindAsync(card.Id);
        deletedCard.Should().BeNull();
    }

    [Theory, AutoData]
    public async Task DeleteCardByIdAsync_WhenCardDoesNotExist_ReturnsFalse(long id)
    {
        // Arrange
        await using var context = _fixture.CreateContext();
        var repository = new CardRepository(context);

        // Act
        var result = await repository.DeleteCardByIdAsync(id);

        // Assert
        result.Should().BeFalse();
    }
}