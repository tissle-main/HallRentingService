using ErrorOr;
using Mediator;
using HallRentingService.Data;
using HallRentingService.WebAPI.Features.HallService.Dtos;
using HallRentingService.Data.Features.HallServices;

namespace HallRentingService.WebAPI.Features.HallServices.Handlers.CreateHallService;

public sealed class CreateHallServiceHandler(AppDbContext thisDbContext) : ICommandHandler<CreateHallServiceCommand, ErrorOr<Guid>>
{
    #region Interfaces
    public async ValueTask<ErrorOr<Guid>> Handle(CreateHallServiceCommand command, CancellationToken cancellationToken)
    {
        if(thisDbContext.HallServices.Select(e => e.Name).Contains(command.HallService.Name))
        {
            return Error.Conflict();
        }
        HallServiceEntity entity = command.HallService.ToEntity();
        await thisDbContext.HallServices.AddAsync(entity, cancellationToken);
        await thisDbContext.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
    #endregion
}