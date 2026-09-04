using ErrorOr;
using Mediator;
using HallRentingService.Data;
using Microsoft.EntityFrameworkCore;
using HallRentingService.Data.Features.Halls;
using HallRentingService.Data.Features.Booking;

namespace HallRentingService.WebAPI.Features.Halls.Handlers.DeleteHalls;

public sealed class DeleteHallsHandler(AppDbContext thisDbContext) : ICommandHandler<DeleteHallsCommand, ErrorOr<Unit>>
{
    #region Interfaces
    public async ValueTask<ErrorOr<Unit>> Handle(DeleteHallsCommand command, CancellationToken cancellationToken)
    {
        if(command.Ids.Length == 0)
        {
            thisDbContext.Bookings.RemoveRange(thisDbContext.Bookings);
            thisDbContext.Halls.RemoveRange(thisDbContext.Halls);
            await thisDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
        Guid[] ids = command.Ids.Distinct().ToArray();
        HallEntity[] halls = await thisDbContext.Halls.AsNoTracking().Where(e => ids.Contains(e.Id)).ToArrayAsync(cancellationToken);
        if(ids.Length > halls.Length)
        {
            Guid[] missingIds = ids.Except(halls.Select(hall => hall.Id)).ToArray();
            return HallErrors.IdsNotFound(missingIds);
        }
        BookingEntity[] bookings = await thisDbContext.Bookings.AsNoTracking().Where(e => ids.Contains(e.HallId)).ToArrayAsync(cancellationToken);
        thisDbContext.Bookings.RemoveRange(bookings);
        thisDbContext.Halls.RemoveRange(halls);
        await thisDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
    #endregion
}