using ErrorOr;
using Mediator;
using HallRentingService.Data;
using HallRentingService.Data.Entities.Halls;
using HallRentingService.WebAPI.Features.Halls.Dtos;
using HallRentingService.WebAPI.Shared.Behaviors.DbTransaction;
using HallRentingService.WebAPI.Features.Hall_HallServices.Handlers;

namespace HallRentingService.WebAPI.Features.Halls.Handlers.CreateHall;

public sealed class CreateHallHandler(AppDbContext thisDbContext, IMediator thisMediator) : ICommandHandler<CreateHallCommand, ErrorOr<Guid>>
{
    #region Interfaces
    public async ValueTask<ErrorOr<Guid>> Handle(CreateHallCommand command, CancellationToken cancellationToken)
    {
        HallEntity entity = command.Hall.ToEntity();
        await thisDbContext.Halls.AddAsync(entity, cancellationToken);
        await thisDbContext.SaveChangesAsync(cancellationToken);

        UpdateHall_HallServices_JoinEntitiesCommand updateHallServicesCommand = new([], entity.HallServices)
        {
            BeginDbTransaction = false
        };
        ErrorOr<Unit> errorOrValue = await thisMediator.Send(updateHallServicesCommand, cancellationToken);
        return errorOrValue.Then(unit => entity.Id);
    }
    #endregion
}