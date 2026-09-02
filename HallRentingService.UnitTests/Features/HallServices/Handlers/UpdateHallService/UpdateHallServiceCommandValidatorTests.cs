using Bogus;
using FluentValidation.TestHelper;
using HallRentingService.Data.Entities.HallServices;
using HallRentingService.WebAPI.Features.HallService.Dtos;
using HallRentingService.WebAPI.Features.HallServices.Dtos;
using HallRentingService.WebAPI.Features.HallServices.Handlers.UpdateHallService;

namespace HallRentingService.UnitTests.Features.HallServices.Handlers.UpdateHallService;

public sealed class UpdateHallServiceCommandValidatorTests
{
    public UpdateHallServiceCommandValidator Validator { get; } = new();

    [Test]
    public void Validator_ShouldPass_WhenInstanceIsValid()
    {
        //Arrange
        UpdateHallServiceCommand command = new(new Faker<HallServiceEntity>().Valid().Generate().ToDto());

        //Act
        TestValidationResult<UpdateHallServiceCommand> result = Validator.TestValidate(command);

        //Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validator_ShouldNotPass_WhenHallServiceHasTooLargeName()
    {
        //Arrange
        UpdateHallServiceCommand command = new(new Faker<HallServiceEntity>().Valid().WithTooLargeName().Generate().ToDto());

        //Act
        TestValidationResult<UpdateHallServiceCommand> result = Validator.TestValidate(command);

        //Assert
        result.ShouldHaveValidationErrorFor(command => command.HallService.Name);
    }
}
