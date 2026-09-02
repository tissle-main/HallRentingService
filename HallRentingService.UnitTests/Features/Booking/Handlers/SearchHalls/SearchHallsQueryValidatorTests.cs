using Bogus;
using FluentValidation.TestHelper;
using HallRentingService.WebAPI.Features.Booking.Handlers.SearchHalls;

namespace HallRentingService.UnitTests.Features.Booking.Handlers.SearchHalls;

public sealed class SearchHallsQueryValidatorTests
{
    public SearchHallsQueryValidator Validator { get; } = new();

    [Test]
    public void Validator_ShouldPass_WhenInstanceIsValid()
    {
        //Arrange
        SearchHallsQuery query = new Faker<SearchHallsQuery>().Valid().Generate();

        //Act
        TestValidationResult<SearchHallsQuery> result = Validator.TestValidate(query);

        //Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validator_ShouldNotPass_WhenInstanceHasExpiredBookingStart()
    {
        //Arrange
        SearchHallsQuery query = new Faker<SearchHallsQuery>().Valid().WithExpiredBookingStart().Generate();

        //Act
        TestValidationResult<SearchHallsQuery> result = Validator.TestValidate(query);

        //Assert
        result.ShouldHaveValidationErrorFor(q => q.BookingStart);
    }

    [Test]
    public void Validator_ShouldNotPass_WhenInstanceHasZeroBookingDuration()
    {
        //Arrange
        SearchHallsQuery query = new Faker<SearchHallsQuery>().Valid().WithZeroBookingDuration().Generate();

        //Act
        TestValidationResult<SearchHallsQuery> result = Validator.TestValidate(query);

        //Assert
        result.ShouldHaveValidationErrorFor(q => q.BookingDuration);
    }

    [Test]
    public void Validator_ShouldNotPass_WhenInstanceHasNegativeBookingDuration()
    {
        //Arrange
        SearchHallsQuery query = new Faker<SearchHallsQuery>().Valid().WithNegativeBookingDuration().Generate();

        //Act
        TestValidationResult<SearchHallsQuery> result = Validator.TestValidate(query);

        //Assert
        result.ShouldHaveValidationErrorFor(q => q.BookingDuration);
    }

    [Test]
    public void Validator_ShouldNotPass_WhenInstanceHasZeroCapacity()
    {
        //Arrange
        SearchHallsQuery query = new Faker<SearchHallsQuery>().Valid().WithZeroCapacity().Generate();

        //Act
        TestValidationResult<SearchHallsQuery> result = Validator.TestValidate(query);

        //Assert
        result.ShouldHaveValidationErrorFor(q => q.Capacity);
    }

    [Test]
    public void Validator_ShouldNotPass_WhenInstanceHasNegativeCapacity()
    {
        //Arrange
        SearchHallsQuery query = new Faker<SearchHallsQuery>().Valid().WithNegativeCapacity().Generate();

        //Act
        TestValidationResult<SearchHallsQuery> result = Validator.TestValidate(query);

        //Assert
        result.ShouldHaveValidationErrorFor(q => q.Capacity);
    }
}