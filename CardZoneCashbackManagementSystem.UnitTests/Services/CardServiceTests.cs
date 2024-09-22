using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;

using CardZoneCashbackManagementSystem.Models;
using CardZoneCashbackManagementSystem.Repositories.Abstractions;
using CardZoneCashbackManagementSystem.Services;

namespace CardZoneCashbackManagementSystem.UnitTests.Services;

public class CardServiceTests
{
    [Theory, InlineAutoData]
    public async Task GetCardsAsync_WhenCalled_CardsReturned(
        ICollection<Card> cards,
        Mock<ICardRepository> cardRepository)
    {
        cardRepository.Setup(cr => cr.GetCardsAsync())
            .ReturnsAsync(cards)
            .Verifiable();

        var sut = new CardService(cardRepository.Object);
        
        var actualCards = await sut.GetCardsAsync();

        cardRepository.Verify(cr => cr.GetCardsAsync(),
            Times.Once());
        actualCards.Should().BeEquivalentTo(cards);
    }

    [Theory, InlineAutoData]
    public async Task GetCardByIdAsync_WhenCalled_IdExists_CardReturned(
        Card card,
        Mock<ICardRepository> cardRepository)
    {
        cardRepository.Setup(cr => cr.GetCardByIdAsync(card.Id))
            .ReturnsAsync(card)
            .Verifiable();

        var sut = new CardService(cardRepository.Object);

        var actualCard = await sut.GetCardByIdAsync(card.Id);
        
        cardRepository.Verify(cr => cr.GetCardByIdAsync(card.Id),
            Times.Once());
        actualCard.Should().NotBeNull()
            .And.BeEquivalentTo(card);
    }

    [Theory, InlineAutoData]
    public async Task AddCardAsync_WhenCalled_NothingReturned(
        Card card,
        Mock<ICardRepository> cardRepository)
    {
        var sut = new CardService(cardRepository.Object);

        await sut.AddCardAsync(card);

        cardRepository.Verify(cr => cr.AddCardAsync(card),
            Times.Once());
    } 
    
    [Theory]
    [InlineAutoData(true)]
    [InlineAutoData(false)]
    public async Task DeleteCardByIdAsync_WhenCalled_SuccessfulRemovalReturnedReturned(
        bool successfullyRemoved,
        Card card,
        Mock<ICardRepository> cardRepository)
    {
        cardRepository.Setup(cr => cr.DeleteCardByIdAsync(card.Id))
            .ReturnsAsync(successfullyRemoved)
            .Verifiable();
        
        var sut = new CardService(cardRepository.Object);

        var actualRemoval = await sut.DeleteCardByIdAsync(card.Id);

        cardRepository.Verify(cr => cr.DeleteCardByIdAsync(card.Id),
            Times.Once());
        actualRemoval.Should().Be(successfullyRemoved);
    } 
}