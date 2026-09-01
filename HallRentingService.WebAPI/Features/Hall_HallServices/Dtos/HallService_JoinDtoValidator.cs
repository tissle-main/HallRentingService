using FluentValidation;

namespace HallRentingService.WebAPI.Features.Hall_HallServices.Dtos;

public sealed class HallService_JoinDtoValidator : AbstractValidator<HallService_JoinDto>
{
    public HallService_JoinDtoValidator()
    {
        base.RuleFor(jd => jd.Price).GreaterThanOrEqualTo(0);
    }
}