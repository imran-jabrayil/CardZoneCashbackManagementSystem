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
        Mock<ICardRepository> cardRepository,
        Mock<IUnitOfWork> unitOfWork)
    {
        cardRepository.Setup(cr => cr.GetCardsAsync())
            .ReturnsAsync(cards)
            .Verifiable();
        unitOfWork.Setup(uow => uow.CardRepository)
            .Returns(cardRepository.Object)
            .Verifiable();
    
        var sut = new CardService(unitOfWork.Object);
        
        var actualCards = await sut.GetCardsAsync();
    
        cardRepository.Verify(cr => cr.GetCardsAsync(), Times.Once());
        unitOfWork.Verify(uow => uow.CardRepository, Times.Once());
        unitOfWork.Verify(uow => uow.SaveAsync(), Times.Never());
        actualCards.Should().BeEquivalentTo(cards);
    }
    
    [Theory, InlineAutoData]
    public async Task GetCardByIdAsync_WhenCalled_IdExists_CardReturned(
        Card card,
        Mock<ICardRepository> cardRepository,
        Mock<IUnitOfWork> unitOfWork)
    {
        cardRepository.Setup(cr => cr.GetCardByIdAsync(card.Id))
            .ReturnsAsync(card)
            .Verifiable();
        unitOfWork.Setup(uow => uow.CardRepository)
            .Returns(cardRepository.Object)
            .Verifiable();
    
        var sut = new CardService(unitOfWork.Object);
    
        var actualCard = await sut.GetCardByIdAsync(card.Id);
        
        cardRepository.Verify(cr => cr.GetCardByIdAsync(card.Id), Times.Once());
        unitOfWork.Verify(uow => uow.CardRepository, Times.Once());
        unitOfWork.Verify(uow => uow.SaveAsync(), Times.Never());
        actualCard.Should().NotBeNull()
            .And.BeEquivalentTo(card);
    }
    
    [Theory, InlineAutoData]
    public async Task AddCardAsync_WhenCalled_NothingReturned(
        Card card,
        Mock<ICardRepository> cardRepository,
        Mock<IUnitOfWork> unitOfWork)
    {
        unitOfWork.Setup(uow => uow.CardRepository)
            .Returns(cardRepository.Object)
            .Verifiable();
        
        var sut = new CardService(unitOfWork.Object);
    
        await sut.AddCardAsync(card);
    
        cardRepository.Verify(cr => cr.AddCardAsync(card), Times.Once());
        unitOfWork.Verify(uow => uow.SaveAsync(), Times.Once());
        unitOfWork.Verify(uow => uow.CardRepository, Times.Once());
    } 
    
    [Theory]
    [InlineAutoData(true)]
    [InlineAutoData(false)]
    public async Task DeleteCardByIdAsync_WhenCalled_SuccessfulRemovalReturnedReturned(
        bool successfullyRemoved,
        Card card,
        Mock<ICardRepository> cardRepository,
        Mock<IUnitOfWork> unitOfWork)
    {
        cardRepository.Setup(cr => cr.DeleteCardByIdAsync(card.Id))
            .ReturnsAsync(successfullyRemoved)
            .Verifiable();
        unitOfWork.Setup(uow => uow.CardRepository)
            .Returns(cardRepository.Object)
            .Verifiable();
        
        var sut = new CardService(unitOfWork.Object);
    
        var actualRemoval = await sut.DeleteCardByIdAsync(card.Id);
    
        cardRepository.Verify(cr => cr.DeleteCardByIdAsync(card.Id), Times.Once());
        unitOfWork.Verify(uow => uow.CardRepository, Times.Once());
        unitOfWork.Verify(uow => uow.SaveAsync(), Times.Once());
        actualRemoval.Should().Be(successfullyRemoved);
    } 
}