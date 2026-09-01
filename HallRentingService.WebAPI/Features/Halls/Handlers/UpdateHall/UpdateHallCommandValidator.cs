using FluentValidation;
using HallRentingService.WebAPI.Features.Halls.Dtos;

namespace HallRentingService.WebAPI.Features.Halls.Handlers.UpdateHall;

public sealed class UpdateHallCommandValidator : AbstractValidator<UpdateHallCommand>
{
    public UpdateHallCommandValidator()
    {
        base.RuleFor(command => command.Hall).SetValidator(new HallDtoValidator());
    }
}