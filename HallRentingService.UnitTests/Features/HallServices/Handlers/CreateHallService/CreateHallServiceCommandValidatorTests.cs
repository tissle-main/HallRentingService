using Bogus;
using FluentValidation.TestHelper;
using HallRentingService.Data.Entities.HallServices;
using HallRentingService.WebAPI.Features.HallService.Dtos;
using HallRentingService.WebAPI.Features.HallServices.Dtos;
using HallRentingService.WebAPI.Features.HallServices.Handlers.CreateHallService;

namespace HallRentingService.UnitTests.Features.HallServices.Handlers.CreateHallService;

public sealed class CreateHallServiceCommandValidatorTests
{
    public CreateHallServiceCommandValidator Validator { get; } = new();

    [Test]
    public void Validator_ShouldPass_WhenInstanceIsValid()
    {
        //Arrange
        CreateHallServiceCommand command = new(new Faker<HallServiceEntity>().Valid().Generate().ToDto());

        //Act
        TestValidationResult<CreateHallServiceCommand> result = Validator.TestValidate(command);

        //Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validator_ShouldNotPass_WhenHallServiceHasTooLargeName()
    {
        //Arrange
        CreateHallServiceCommand command = new(new Faker<HallServiceEntity>().Valid().WithTooLargeName().Generate().ToDto());

        //Act
        TestValidationResult<CreateHallServiceCommand> result = Validator.TestValidate(command);

        //Assert
        result.ShouldHaveValidationErrorFor(command => command.HallService.Name);
    }
}