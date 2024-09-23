using CardZoneCashbackManagementSystem.Utils;
using FluentAssertions;

namespace CardZoneCashbackManagementSystem.UnitTests.Utils;

public class CardUtilsTests
{
    [Theory]
    [InlineData("4532015112830366", true)]
    [InlineData("6011514433546201", true)]
    [InlineData("378282246310005", true)]
    [InlineData("371449635398431", true)]
    public void ValidateLuhn_WithValidCardNumber_ReturnsTrue(string cardNumber, bool expected)
    {
        // Act
        var result = CardUtils.ValidateLuhn(cardNumber);

        // Assert
        result.Should().Be(expected);
    }
}