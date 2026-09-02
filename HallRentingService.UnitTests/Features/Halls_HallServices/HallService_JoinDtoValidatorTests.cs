using Bogus;
using FluentValidation.TestHelper;
using HallRentingService.Data.Entities.Halls_HallServices;
using HallRentingService.WebAPI.Features.Halls_HallServices;
using HallRentingService.WebAPI.Features.Hall_HallServices.Dtos;

namespace HallRentingService.UnitTests.Features.Halls_HallServices;

public sealed class HallService_JoinDtoValidatorTests
{
    public HallService_JoinDtoValidator Validator { get; } = new();

    [Test]
    public void Validator_ShouldPass_WhenInstanceIsValid()
    {
        //Arrange
        HallService_JoinDto dto = new Faker<Hall_HallService_JoinEntity>().Valid().Generate().ToHallServiceDto();

        //Act
        TestValidationResult<HallService_JoinDto> result = Validator.TestValidate(dto);

        //Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validator_ShouldNotPass_WhenInstanceHasNegativePrice()
    {
        //Arrange
        HallService_JoinDto dto = new Faker<Hall_HallService_JoinEntity>().Valid().WithNegativePrice().Generate().ToHallServiceDto();

        //Act
        TestValidationResult<HallService_JoinDto> result = Validator.TestValidate(dto);

        //Assert
        result.ShouldHaveValidationErrorFor(dto => dto.Price);
    }
}