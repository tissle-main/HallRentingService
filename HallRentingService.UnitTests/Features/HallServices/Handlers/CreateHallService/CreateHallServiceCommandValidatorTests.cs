using Bogus;
using FluentValidation.TestHelper;
using HallRentingService.WebAPI.Features.HallService.Dtos;
using HallRentingService.WebAPI.Features.HallServices.Dtos;
using HallRentingService.UnitTests.Features.HallServices.Dtos;
using HallRentingService.WebAPI.Features.HallServices.Handlers.CreateHallService;
using HallRentingService.Data.Features.HallServices;

namespace HallRentingService.UnitTests.Features.HallServices.Handlers.CreateHallService;

public sealed class CreateHallServiceCommandValidatorTests
{
    public CreateHallServiceCommandValidator Validator { get; } = new();

    [Test]
    [DependsOn<HallServiceDtoValidatorTests>]
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
    [DependsOn<HallServiceDtoValidatorTests>]
    public void Validator_ShouldNotPass_WhenInstanceIsInvalid()
    {
        //Arrange
        CreateHallServiceCommand command = new(new Faker<HallServiceEntity>().Valid().WithTooLargeName().Generate().ToDto());

        //Act
        TestValidationResult<CreateHallServiceCommand> result = Validator.TestValidate(command);

        //Assert
        result.ShouldHaveValidationErrors();
    }
}