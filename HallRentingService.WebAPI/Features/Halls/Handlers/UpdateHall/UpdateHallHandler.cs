using ErrorOr;
using Mediator;
using HallRentingService.Data;
using Microsoft.EntityFrameworkCore;
using HallRentingService.Data.Features.Halls;
using HallRentingService.Data.Features.HallServices;
using HallRentingService.WebAPI.Features.Halls.Dtos;
using HallRentingService.WebAPI.Features.HallServices;

namespace HallRentingService.WebAPI.Features.Halls.Handlers.UpdateHall;

public sealed class UpdateHallHandler(AppDbContext thisDbContext) : ICommandHandler<UpdateHallCommand, ErrorOr<Unit>>
{
    #region Interfaces
    public async ValueTask<ErrorOr<Unit>> Handle(UpdateHallCommand command, CancellationToken cancellationToken)
    {
        Guid[] hallServiceIds = command.Hall.HallServices.Select(hs => hs.HallServiceId).ToArray();
        HallServiceEntity[] hallServices = thisDbContext.HallServices.AsNoTracking().Where(hs => hallServiceIds.Contains(hs.Id)).ToArray();
        if(hallServiceIds.Length > hallServices.Length)
        {
            Guid[] missingIds = hallServiceIds.Except(hallServices.Select(h => h.Id)).ToArray();
            return HallServiceErrors.IdsNotFound(missingIds);
        }
        HallEntity? oldEntity = await thisDbContext.Halls.Include(e => e.HallServices).FirstOrDefaultAsync(
            e => e.Id == command.Hall.Id,
            cancellationToken
        );
        if(oldEntity is null)
        {
            return HallErrors.IdsNotFound([command.Hall.Id]);
        }

        command.Hall.ToEntity().MapToEntity(oldEntity);
        await thisDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
    #endregion
}