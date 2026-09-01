using FluentValidation;
using HallRentingService.Data.Entities.Halls;
using HallRentingService.WebAPI.Features.Hall_HallServices.Dtos;

namespace HallRentingService.WebAPI.Features.Halls.Dtos;

public sealed class HallDtoValidator : AbstractValidator<HallDto>
{
    public HallDtoValidator()
    {
        base.RuleFor(dto => dto.Name).MaximumLength(HallEntityConstants.NameMaxLength);
        base.RuleFor(dto => dto.Capacity).GreaterThanOrEqualTo(0);
        base.RuleFor(dto => dto.BasePrice).GreaterThanOrEqualTo(0);
        base.RuleForEach(dto => dto.HallServices).SetValidator(new HallService_JoinDtoValidator());
    }
}