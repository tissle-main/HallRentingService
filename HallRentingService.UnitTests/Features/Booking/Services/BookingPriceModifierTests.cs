using HallRentingService.WebAPI.Features.Booking.Services;

namespace HallRentingService.UnitTests.Features.Booking.Services;

public sealed class BookingPriceModifierTests
{
    public BookingPriceModifier Modifier { get; } = new();

    [Test]
    public async Task ApplyModifiers_ShouldReturnZero_WhenBookingDurationIsZero()
    {
        //Arrange
        DateTime bookingStart = new(2026, 9, 4, 10, 0, 0);

        //Act
        decimal result = Modifier.ApplyModifiers(100, bookingStart, TimeSpan.Zero);

        //Assert
        await Assert.That(result).IsEqualTo(0);
    }

    [Test]
    public async Task ApplyModifiers_ShouldReturnZero_WhenBookingDurationIsNegative()
    {
        //Arrange
        DateTime bookingStart = new(2026, 9, 4, 10, 0, 0);

        //Act
        decimal result = Modifier.ApplyModifiers(100, bookingStart, TimeSpan.FromHours(-1));

        //Assert
        await Assert.That(result).IsEqualTo(0);
    }

    [Test]
    [Arguments(9, 3, 300)]
    [Arguments(14, 4, 400)]
    [Arguments(23, 7, 700)]
    public async Task ApplyModifiers_ShouldApplyNoModifier_DuringRegularHours(int startHour, int duration, float finalPrice)
    {
        //Arrange
        DateTime bookingStart = new(2026, 9, 4, startHour, 0, 0);

        //Act
        decimal result = Modifier.ApplyModifiers(100, bookingStart, TimeSpan.FromHours(duration));

        //Assert
        await Assert.That(result).IsEqualTo(finalPrice);
    }

    [Test]
    public async Task ApplyModifiers_ShouldApplyMorningDiscount()
    {
        //Arrange
        DateTime bookingStart = new(2026, 9, 4, 6, 0, 0);

        //Act
        decimal result = Modifier.ApplyModifiers(100, bookingStart, TimeSpan.FromHours(3));

        //Assert
        await Assert.That(result).IsEqualTo(270);
    }

    [Test]
    public async Task ApplyModifiers_ShouldApplyLunchSurcharge()
    {
        //Arrange
        DateTime bookingStart = new(2026, 9, 4, 12, 0, 0);

        //Act
        decimal result = Modifier.ApplyModifiers(100, bookingStart, TimeSpan.FromHours(2));

        //Assert
        await Assert.That(result).IsEqualTo(230);
    }

    [Test]
    public async Task ApplyModifiers_ShouldApplyEveningDiscount()
    {
        //Arrange
        DateTime bookingStart = new(2026, 9, 4, 18, 0, 0);

        //Act
        decimal result = Modifier.ApplyModifiers(100, bookingStart, TimeSpan.FromHours(5));

        //Assert
        await Assert.That(result).IsEqualTo(400);
    }

    [Test]
    public async Task ApplyModifiers_ShouldSplitPrice_WhenBookingCrossesModifierPeriods()
    {
        //Arrange
        DateTime bookingStart = new(2026, 9, 4, 6, 0, 0);

        //Act
        decimal result = Modifier.ApplyModifiers(100, bookingStart, TimeSpan.FromHours(24));

        //Assert
        await Assert.That(result).IsEqualTo(2300);
    }
}