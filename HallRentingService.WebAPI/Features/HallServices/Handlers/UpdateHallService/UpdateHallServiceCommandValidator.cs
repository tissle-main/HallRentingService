using FluentValidation;
using HallRentingService.WebAPI.Features.HallServices.Dtos;

namespace HallRentingService.WebAPI.Features.HallServices.Handlers.UpdateHallService;

public sealed class UpdateHallServiceCommandValidator : AbstractValidator<UpdateHallServiceCommand>
{
    public UpdateHallServiceCommandValidator()
    {
        base.RuleFor(command => command.HallService).SetValidator(new HallServiceDtoValidator());
    }
}
