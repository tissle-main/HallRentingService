using FluentValidation;
using HallRentingService.Data.Entities.Halls;
using HallRentingService.WebAPI.Features.Halls_HallServices;

namespace HallRentingService.WebAPI.Features.Halls.Dtos;

public sealed class HallDtoValidator : AbstractValidator<HallDto>
{
    public HallDtoValidator()
    {
        base.RuleFor(dto => dto.Name).NotEmpty().MaximumLength(HallEntityConstants.NameMaxLength);
        base.RuleFor(dto => dto.Capacity).GreaterThan(0);
        base.RuleFor(dto => dto.PricePerHour).GreaterThanOrEqualTo(0);
        base.RuleForEach(dto => dto.HallServices).SetValidator(new HallService_JoinDtoValidator());
    }
}