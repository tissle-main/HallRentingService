using Bogus;
using FluentValidation.TestHelper;
using HallRentingService.WebAPI.Features.Booking.Handlers.BookHall;

namespace HallRentingService.UnitTests.Features.Booking.Handlers.BookHall;

public sealed class BookHallCommandValidatorTests
{
    public BookHallCommandValidator Validator { get; } = new();

    [Test]
    public void Validator_ShouldPass_WhenInstanceIsValid()
    {
        //Arrange
        BookHallCommand command = new Faker<BookHallCommand>().Valid().Generate();

        //Act
        TestValidationResult<BookHallCommand> result = Validator.TestValidate(command);

        //Assert        
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validator_ShouldNotPass_WhenInstanceHasExpiredBookingStart()
    {
        //Arrange
        BookHallCommand command = new Faker<BookHallCommand>().Valid().WithExpiredBookingStart().Generate();

        //Act
        TestValidationResult<BookHallCommand> result = Validator.TestValidate(command);

        //Assert
        result.ShouldHaveValidationErrorFor(command => command.BookingStart);
    }

    [Test]
    public void Validator_ShouldNotPass_WhenInstanceHasZeroBookingDuration()
    {
        //Arrange
        BookHallCommand command = new Faker<BookHallCommand>().Valid().WithZeroBookingDuration().Generate();

        //Act
        TestValidationResult<BookHallCommand> result = Validator.TestValidate(command);

        //Assert
        result.ShouldHaveValidationErrorFor(command => command.BookingDuration);
    }

    [Test]
    public void Validator_ShouldNotPass_WhenInstanceHasNegativeBookingDuration()
    {
        //Arrange
        BookHallCommand command = new Faker<BookHallCommand>().Valid().WithNegativeBookingDuration().Generate();

        //Act
        TestValidationResult<BookHallCommand> result = Validator.TestValidate(command);

        //Assert
        result.ShouldHaveValidationErrorFor(command => command.BookingDuration);
    }
}