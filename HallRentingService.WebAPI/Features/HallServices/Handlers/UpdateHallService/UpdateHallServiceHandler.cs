using ErrorOr;
using Mediator;
using HallRentingService.Data;
using Microsoft.EntityFrameworkCore;
using HallRentingService.Data.Entities.HallServices;
using HallRentingService.WebAPI.Features.HallService.Dtos;

namespace HallRentingService.WebAPI.Features.HallServices.Handlers.UpdateHallService;

public sealed class UpdateHallServiceHandler(AppDbContext thisDbContext) : ICommandHandler<UpdateHallServiceCommand, ErrorOr<Unit>>
{
    #region Interfaces
    public async ValueTask<ErrorOr<Unit>> Handle(UpdateHallServiceCommand command, CancellationToken cancellationToken)
    {
        if(thisDbContext.HallServices.Select(e => e.Name).Contains(command.HallService.Name))
        {
            return Error.Conflict();
        }
        HallServiceEntity? entity = await thisDbContext.HallServices.FirstOrDefaultAsync(
            e => e.Id == command.HallService.Id,
            cancellationToken
        );
        if(entity is null)
        {
            return Error.NotFound();
        }     
        command.HallService.MapToEntity(entity);
        await thisDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
    #endregion
}