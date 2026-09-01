using FluentValidation;

namespace HallRentingService.WebAPI.Features.Hall_HallServices.Dtos;

public sealed class Hall_JoinDtoValidator : AbstractValidator<Hall_JoinDto>
{
    public Hall_JoinDtoValidator()
    {
        base.RuleFor(jd => jd.Price).GreaterThanOrEqualTo(0);
    }
}