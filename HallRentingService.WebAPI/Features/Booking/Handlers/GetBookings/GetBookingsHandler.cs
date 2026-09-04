using ErrorOr;
using Mediator;
using HallRentingService.Data;
using Microsoft.EntityFrameworkCore;
using HallRentingService.WebAPI.Features.Booking.Dtos;

namespace HallRentingService.WebAPI.Features.Booking.Handlers.GetBookings;

public sealed class GetBookingsHandler(AppDbContext thisDbContext) : IQueryHandler<GetBookingsQuery, ErrorOr<IEnumerable<BookingDto>>>
{
    #region Interfaces
    public async ValueTask<ErrorOr<IEnumerable<BookingDto>>> Handle(GetBookingsQuery query, CancellationToken cancellationToken)
    {
        if(query.Ids.Length == 0)
        {
            return await thisDbContext.Bookings.AsNoTracking().ProjectToDto().ToArrayAsync(cancellationToken);
        }
        Guid[] ids = query.Ids.Distinct().ToArray();
        BookingDto[] bookings = await thisDbContext.Bookings.AsNoTracking().Where(b => ids.Contains(b.Id)).ProjectToDto().ToArrayAsync(cancellationToken);
        if(ids.Length > bookings.Length)
        {
            return Error.NotFound();
        }
        return bookings;
    }
    #endregion
}