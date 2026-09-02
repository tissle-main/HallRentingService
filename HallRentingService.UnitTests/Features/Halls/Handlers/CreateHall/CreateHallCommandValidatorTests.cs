using Bogus;
using FluentValidation.TestHelper;
using HallRentingService.Data.Entities.Halls;
using HallRentingService.WebAPI.Features.Halls.Dtos;
using HallRentingService.WebAPI.Features.Halls.Handlers.CreateHall;

namespace HallRentingService.UnitTests.Features.Halls.Handlers.CreateHall;

public sealed class CreateHallCommandValidatorTests
{
    public CreateHallCommandValidator Validator { get; } = new();

    [Test]
    public void Validator_ShouldPass_WhenInstanceIsValid()
    {
        //Arrange
        CreateHallCommand command = new(new Faker<HallEntity>().Valid().Generate().ToDto());

        //Act
        TestValidationResult<CreateHallCommand> result = Validator.TestValidate(command);

        //Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validator_ShouldNotPass_WhenHallHasInvalidCapacity()
    {
        //Arrange
        CreateHallCommand command = new(new Faker<HallEntity>().Valid().WithZeroCapacity().Generate().ToDto());

        //Act
        TestValidationResult<CreateHallCommand> result = Validator.TestValidate(command);

        //Assert
        result.ShouldHaveValidationErrorFor(command => command.Hall.Capacity);
    }
}