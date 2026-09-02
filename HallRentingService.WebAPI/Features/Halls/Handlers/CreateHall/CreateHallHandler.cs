using ErrorOr;
using Mediator;
using HallRentingService.Data;
using HallRentingService.Data.Entities.Halls;
using HallRentingService.WebAPI.Features.Halls.Dtos;

namespace HallRentingService.WebAPI.Features.Halls.Handlers.CreateHall;

public sealed class CreateHallHandler(AppDbContext thisDbContext) : ICommandHandler<CreateHallCommand, ErrorOr<Guid>>
{
    #region Interfaces
    public async ValueTask<ErrorOr<Guid>> Handle(CreateHallCommand command, CancellationToken cancellationToken)
    {
        HallEntity entity = command.Hall.ToEntity();
        await thisDbContext.Halls.AddAsync(entity, cancellationToken);
        await thisDbContext.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
    #endregion
}