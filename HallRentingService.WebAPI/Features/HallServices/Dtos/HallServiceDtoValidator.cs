using FluentValidation;
using HallRentingService.Data.Entities.HallServices;

namespace HallRentingService.WebAPI.Features.HallServices.Dtos;

public sealed class HallServiceDtoValidator : AbstractValidator<HallServiceDto>
{
    public HallServiceDtoValidator()
    {
        base.RuleFor(e => e.Name).MaximumLength(HallServiceEntityConstants.NameMaxLength);
    }
}