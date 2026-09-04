using ErrorOr;
using Mediator;
using HallRentingService.Data;
using Microsoft.EntityFrameworkCore;
using HallRentingService.Data.Features.Halls;
using HallRentingService.WebAPI.Features.Halls.Dtos;

namespace HallRentingService.WebAPI.Features.Halls.Handlers.UpdateHall;

public sealed class UpdateHallHandler(AppDbContext thisDbContext) : ICommandHandler<UpdateHallCommand, ErrorOr<Unit>>
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
        command.Hall.ToEntity().MapToEntity(oldEntity);
        await thisDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
    #endregion
}