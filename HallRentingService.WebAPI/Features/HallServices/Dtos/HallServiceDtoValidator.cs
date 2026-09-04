using FluentValidation;
using HallRentingService.Data.Features.HallServices;

namespace HallRentingService.WebAPI.Features.HallServices.Dtos;

public sealed class HallServiceDtoValidator : AbstractValidator<HallServiceDto>
{
    public HallServiceDtoValidator()
    {
        base.RuleFor(e => e.Name).NotEmpty().MaximumLength(HallServiceEntityConstants.NameMaxLength);
    }
}