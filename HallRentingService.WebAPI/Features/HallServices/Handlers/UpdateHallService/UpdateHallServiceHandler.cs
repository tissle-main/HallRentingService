using ErrorOr;
using Mediator;
using HallRentingService.Data;
using Microsoft.EntityFrameworkCore;
using HallRentingService.WebAPI.Features.HallService.Dtos;
using HallRentingService.Data.Features.HallServices;

namespace HallRentingService.WebAPI.Features.HallServices.Handlers.UpdateHallService;

public sealed class UpdateHallServiceHandler(AppDbContext thisDbContext) : ICommandHandler<UpdateHallServiceCommand, ErrorOr<Unit>>
{
    #region Interfaces
    public async ValueTask<ErrorOr<Unit>> Handle(UpdateHallServiceCommand command, CancellationToken cancellationToken)
    {
        HallServiceEntity? entity = await thisDbContext.HallServices.FirstOrDefaultAsync(
            e => e.Id == command.HallService.Id,
            cancellationToken
        );
        if(entity is null)
        {
            return Error.NotFound();
        }
        if(entity.Name != command.HallService.Name && thisDbContext.HallServices.Select(e => e.Name).Contains(command.HallService.Name))
        {
            return Error.Conflict();
        }
        command.HallService.MapToEntity(entity);
        await thisDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
    #endregion
}