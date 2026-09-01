using ErrorOr;
using Mediator;
using HallRentingService.Data;
using Microsoft.EntityFrameworkCore;
using HallRentingService.Data.Entities.Halls;
using HallRentingService.WebAPI.Features.Halls.Dtos;
using HallRentingService.WebAPI.Shared.Behaviors.DbTransaction;
using HallRentingService.WebAPI.Features.Hall_HallServices.Handlers;

namespace HallRentingService.WebAPI.Features.Halls.Handlers.UpdateHall;

public sealed class UpdateHallHandler(AppDbContext thisDbContext, IMediator thisMediator) : ICommandHandler<UpdateHallCommand, ErrorOr<Unit>>
{
    #region Interfaces
    public async ValueTask<ErrorOr<Unit>> Handle(UpdateHallCommand command, CancellationToken cancellationToken)
    {
        HallEntity? oldEntity = await thisDbContext.Halls.Include(e => e.HallServices).FirstOrDefaultAsync(
            e => e.Id == command.Hall.Id,
            cancellationToken
        );
        if(oldEntity is null)
        {
            return Error.NotFound();
        }
        HallEntity newEntity = command.Hall.ToEntity();

        UpdateHall_HallServices_JoinEntitiesCommand updateHallServicesCommand = new(oldEntity.HallServices, newEntity.HallServices)
        {
            BeginDbTransaction = false
        };
        ErrorOr<Unit> errorOrValue = await thisMediator.Send(updateHallServicesCommand, cancellationToken);
        if(errorOrValue.IsError)
        {
            return errorOrValue;
        }

        thisDbContext.Attach(oldEntity);
        newEntity.MapToEntity(oldEntity);
        await thisDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
    #endregion
}