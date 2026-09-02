using Bogus;
using FluentValidation.TestHelper;
using HallRentingService.Data.Entities.Halls;
using HallRentingService.WebAPI.Features.Halls.Dtos;
using HallRentingService.WebAPI.Features.Halls.Handlers.UpdateHall;

namespace HallRentingService.UnitTests.Features.Halls.Handlers.UpdateHall;

public sealed class UpdateHallCommandValidatorTests
{
    public UpdateHallCommandValidator Validator { get; } = new();

    [Test]
    public void Validator_ShouldPass_WhenInstanceIsValid()
    {
        //Arrange
        UpdateHallCommand command = new(new Faker<HallEntity>().Valid().Generate().ToDto());

        //Act
        TestValidationResult<UpdateHallCommand> result = Validator.TestValidate(command);

        //Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validator_ShouldNotPass_WhenHallHasInvalidCapacity()
    {
        //Arrange
        UpdateHallCommand command = new(new Faker<HallEntity>().Valid().WithZeroCapacity().Generate().ToDto());

        //Act
        TestValidationResult<UpdateHallCommand> result = Validator.TestValidate(command);

        //Assert
        result.ShouldHaveValidationErrorFor(command => command.Hall.Capacity);
    }
}