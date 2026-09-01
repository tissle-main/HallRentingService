using FluentValidation;
using HallRentingService.WebAPI.Features.Halls.Dtos;

namespace HallRentingService.WebAPI.Features.Halls.Handlers.CreateHall;

public sealed class CreateHallCommandValidator : AbstractValidator<CreateHallCommand>
{
    public CreateHallCommandValidator()
    {
        base.RuleFor(command => command.Hall).SetValidator(new HallDtoValidator());
    }
}