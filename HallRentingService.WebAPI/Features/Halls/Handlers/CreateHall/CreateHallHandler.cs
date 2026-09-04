using ErrorOr;
using Mediator;
using HallRentingService.Data;
using Microsoft.EntityFrameworkCore;
using HallRentingService.Data.Features.Halls;
using HallRentingService.Data.Features.HallServices;
using HallRentingService.WebAPI.Features.Halls.Dtos;
using HallRentingService.WebAPI.Features.HallServices;

namespace HallRentingService.WebAPI.Features.Halls.Handlers.CreateHall;

public sealed class CreateHallHandler(AppDbContext thisDbContext) : ICommandHandler<CreateHallCommand, ErrorOr<Guid>>
{
    #region Interfaces
    public async ValueTask<ErrorOr<Guid>> Handle(CreateHallCommand command, CancellationToken cancellationToken)
    {
        Guid[] hallServiceIds = command.Hall.HallServices.Select(hs => hs.HallServiceId).ToArray();
        HallServiceEntity[] hallServices = thisDbContext.HallServices.AsNoTracking().Where(hs => hallServiceIds.Contains(hs.Id)).ToArray();
        if(hallServiceIds.Length > hallServices.Length)
        {
            Guid[] missingIds = hallServiceIds.Except(hallServices.Select(h => h.Id)).ToArray();
            return HallServiceErrors.IdsNotFound(missingIds);
        }

        HallEntity entity = command.Hall.ToEntity();
        await thisDbContext.Halls.AddAsync(entity, cancellationToken);
        await thisDbContext.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
    #endregion
}