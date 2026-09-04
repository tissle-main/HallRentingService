using Bogus;
using FluentValidation.TestHelper;
using HallRentingService.Data.Features.Halls;
using HallRentingService.WebAPI.Features.Halls.Dtos;
using HallRentingService.UnitTests.Features.Halls.Dtos;
using HallRentingService.WebAPI.Features.Halls.Handlers.UpdateHall;

namespace HallRentingService.UnitTests.Features.Halls.Handlers.UpdateHall;

public sealed class UpdateHallCommandValidatorTests
{
    public UpdateHallCommandValidator Validator { get; } = new();

    [Test]
    [DependsOn<HallDtoValidatorTests>]
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
    [DependsOn<HallDtoValidatorTests>]
    public void Validator_ShouldNotPass_WhenInstanceIsInvalid()
    {
        //Arrange
        UpdateHallCommand command = new(new Faker<HallEntity>().Valid().WithZeroCapacity().Generate().ToDto());

        //Act
        TestValidationResult<UpdateHallCommand> result = Validator.TestValidate(command);

        //Assert
        result.ShouldHaveValidationErrors();
    }
}