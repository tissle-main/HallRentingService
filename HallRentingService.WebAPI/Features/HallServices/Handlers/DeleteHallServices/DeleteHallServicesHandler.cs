using ErrorOr;
using Mediator;
using HallRentingService.Data;
using Microsoft.EntityFrameworkCore;
using HallRentingService.Data.Features.HallServices;

namespace HallRentingService.WebAPI.Features.HallServices.Handlers.DeleteHallServices;

public sealed class DeleteHallServicesHandler(AppDbContext thisDbContext) : ICommandHandler<DeleteHallServicesCommand, ErrorOr<Unit>>
{
    #region Interfaces
    public async ValueTask<ErrorOr<Unit>> Handle(DeleteHallServicesCommand command, CancellationToken cancellationToken)
    {
        if(command.Ids.Length == 0)
        {
            thisDbContext.HallServices.RemoveRange(thisDbContext.HallServices);
            await thisDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
        Guid[] ids = command.Ids.Distinct().ToArray();
        HallServiceEntity[] hallServices = await thisDbContext.HallServices.AsNoTracking().Where(e => ids.Contains(e.Id)).ToArrayAsync(cancellationToken);
        if(ids.Length > hallServices.Length)
        {
            return Error.NotFound();
        }
        thisDbContext.HallServices.RemoveRange(hallServices);
        await thisDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
    #endregion
}