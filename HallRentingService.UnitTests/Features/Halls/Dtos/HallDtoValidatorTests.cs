using Bogus;
using FluentValidation.TestHelper;
using HallRentingService.Data.Entities.Halls;
using HallRentingService.WebAPI.Features.Halls.Dtos;
using HallRentingService.Data.Entities.Halls_HallServices;
using HallRentingService.WebAPI.Features.Halls_HallServices;
using HallRentingService.UnitTests.Features.Halls_HallServices;

namespace HallRentingService.UnitTests.Features.Halls.Dtos;

public sealed class HallDtoValidatorTests
{
    public HallDtoValidator Validator { get; } = new();

    [Test]
    public void Validator_ShouldPass_WhenInstanceIsValid()
    {
        //Arrange
        HallDto dto = new Faker<HallEntity>().Valid().Generate().ToDto();

        //Act
        TestValidationResult<HallDto> result = Validator.TestValidate(dto);

        //Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validator_ShouldNotPass_WhenInstanceHasEmptyName()
    {
        //Arrange
        HallDto dto = new Faker<HallEntity>().Valid().WithEmptyName().Generate().ToDto();

        //Act
        TestValidationResult<HallDto> result = Validator.TestValidate(dto);

        //Assert
        result.ShouldHaveValidationErrorFor(dto => dto.Name);
    }

    [Test]
    public void Validator_ShouldNotPass_WhenInstanceHasTooLargeName()
    {
        //Arrange
        HallDto dto = new Faker<HallEntity>().Valid().WithTooLargeName().Generate().ToDto();

        //Act
        TestValidationResult<HallDto> result = Validator.TestValidate(dto);

        //Assert
        result.ShouldHaveValidationErrorFor(dto => dto.Name);
    }

    [Test]
    public void Validator_ShouldNotPass_WhenInstanceHasZeroCapacity()
    {
        //Arrange
        HallDto dto = new Faker<HallEntity>().Valid().WithZeroCapacity().Generate().ToDto();

        //Act
        TestValidationResult<HallDto> result = Validator.TestValidate(dto);

        //Assert
        result.ShouldHaveValidationErrorFor(dto => dto.Capacity);
    }

    [Test]
    public void Validator_ShouldNotPass_WhenInstanceHasNegativeCapacity()
    {
        //Arrange
        HallDto dto = new Faker<HallEntity>().Valid().WithNegativeCapacity().Generate().ToDto();

        //Act
        TestValidationResult<HallDto> result = Validator.TestValidate(dto);

        //Assert
        result.ShouldHaveValidationErrorFor(dto => dto.Capacity);
    }

    [Test]
    public void Validator_ShouldNotPass_WhenInstanceHasNegativePricePerHour()
    {
        //Arrange
        HallDto dto = new Faker<HallEntity>().Valid().WithNegativePricePerHour().Generate().ToDto();

        //Act
        TestValidationResult<HallDto> result = Validator.TestValidate(dto);

        //Assert
        result.ShouldHaveValidationErrorFor(dto => dto.PricePerHour);
    }

    [Test]
    [DependsOn<HallService_JoinDtoValidatorTests>]
    public void Validator_ShouldNotPass_WhenInstanceHasInvalidHallServices()
    {
        //Arrange
        List<Hall_HallService_JoinEntity> jes = new Faker<Hall_HallService_JoinEntity>().Valid().WithNegativePrice().GenerateLazy(1).ToList();  
        HallDto dto = new Faker<HallEntity>().Valid().WithHallServices(jes).Generate().ToDto();

        //Act
        TestValidationResult<HallDto> result = Validator.TestValidate(dto);

        //Assert
        result.ShouldHaveValidationErrors();
    }
}