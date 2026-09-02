using Bogus;
using FluentValidation.TestHelper;
using HallRentingService.Data.Entities.HallServices;
using HallRentingService.WebAPI.Features.HallService.Dtos;
using HallRentingService.WebAPI.Features.HallServices.Dtos;

namespace HallRentingService.UnitTests.Features.HallServices.Dtos;

public sealed class HallServiceDtoValidatorTests
{
    public HallServiceDtoValidator Validator { get; } = new();

    [Test]
    public void Validator_ShouldPass_WhenInstanceIsValid()
    {
        //Arrange
        HallServiceDto dto = new Faker<HallServiceEntity>().Valid().Generate().ToDto();

        //Act
        TestValidationResult<HallServiceDto> result = Validator.TestValidate(dto);

        //Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validator_ShouldNotPass_WhenInstanceHasTooLargeName()
    {
        //Arrange
        HallServiceDto dto = new Faker<HallServiceEntity>().Valid().WithTooLargeName().Generate().ToDto();

        //Act
        TestValidationResult<HallServiceDto> result = Validator.TestValidate(dto);

        //Assert
        result.ShouldHaveValidationErrorFor(dto => dto.Name);
    }
}