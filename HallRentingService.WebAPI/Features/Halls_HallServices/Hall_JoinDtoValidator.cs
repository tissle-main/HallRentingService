using FluentValidation;

namespace HallRentingService.WebAPI.Features.Halls_HallServices;

public sealed class Hall_JoinDtoValidator : AbstractValidator<Hall_JoinDto>
{
    public Hall_JoinDtoValidator()
    {
        base.RuleFor(jd => jd.Price).GreaterThanOrEqualTo(0);
    }
}