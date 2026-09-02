using ErrorOr;
using Mediator;
using HallRentingService.Data;
using Microsoft.EntityFrameworkCore;
using HallRentingService.WebAPI.Features.Halls.Dtos;

namespace HallRentingService.WebAPI.Features.Booking.Handlers.SearchHalls;

public sealed class SearchHallsHandler(AppDbContext thisDbContext) : IQueryHandler<SearchHallsQuery, ErrorOr<IEnumerable<HallDto>>>
{
    #region Interfaces
    public async ValueTask<ErrorOr<IEnumerable<HallDto>>> Handle(SearchHallsQuery query, CancellationToken cancellationToken)
    {
        DateTime bookingEndDateTime = query.BookingStart.Add(query.BookingDuration);
        return await thisDbContext.Halls.AsNoTracking().Include(e => e.HallServices).Include(e => e.Bookings)
            .Where(e => e.Capacity <= query.Capacity)
            .Where(
                e => !e.Bookings.Any(
                    booking => booking.BookingStart < bookingEndDateTime && query.BookingStart < booking.BookingStart.Add(booking.BookingDuration)
                )
            )
            .ProjectToDto()
            .ToArrayAsync(cancellationToken);
    }
    #endregion
}