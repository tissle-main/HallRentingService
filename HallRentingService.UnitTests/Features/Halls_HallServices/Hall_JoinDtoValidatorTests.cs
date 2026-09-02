using Bogus;
using FluentValidation.TestHelper;
using HallRentingService.Data.Entities.Halls_HallServices;
using HallRentingService.WebAPI.Features.Halls_HallServices;
using HallRentingService.WebAPI.Features.Hall_HallServices.Dtos;

namespace HallRentingService.UnitTests.Features.Halls_HallServices;

public sealed class Hall_JoinDtoValidatorTests
{
    public Hall_JoinDtoValidator Validator { get; } = new();

    [Test]
    public void Validator_ShouldPass_WhenInstanceIsValid()
    {
        //Arrange
        Hall_JoinDto dto = new Faker<Hall_HallService_JoinEntity>().Valid().Generate().ToHallDto();

        //Act
        TestValidationResult<Hall_JoinDto> result = Validator.TestValidate(dto);

        //Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validator_ShouldNotPass_WhenInstanceHasNegativePrice()
    {
        //Arrange
        Hall_JoinDto dto = new Faker<Hall_HallService_JoinEntity>().Valid().WithNegativePrice().Generate().ToHallDto();

        //Act
        TestValidationResult<Hall_JoinDto> result = Validator.TestValidate(dto);

        //Assert
        result.ShouldHaveValidationErrorFor(dto => dto.Price);
    }
}