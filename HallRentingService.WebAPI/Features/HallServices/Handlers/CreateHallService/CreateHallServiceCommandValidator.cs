using FluentValidation;
using HallRentingService.WebAPI.Features.HallServices.Dtos;

namespace HallRentingService.WebAPI.Features.HallServices.Handlers.CreateHallService;

public sealed class CreateHallServiceCommandValidator : AbstractValidator<CreateHallServiceCommand>
{
    public CreateHallServiceCommandValidator()
    {
        base.RuleFor(command => command.HallService).SetValidator(new HallServiceDtoValidator());
    }
}