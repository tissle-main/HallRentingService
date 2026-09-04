using ErrorOr;
using Mediator;
using HallRentingService.Data;
using Microsoft.EntityFrameworkCore;
using HallRentingService.Data.Features.Halls;
using HallRentingService.WebAPI.Features.Halls.Dtos;

namespace HallRentingService.WebAPI.Features.Booking.Handlers.SearchHalls;

public sealed class SearchHallsHandler(AppDbContext thisDbContext) : IQueryHandler<SearchHallsQuery, ErrorOr<IEnumerable<HallDto>>>
{
    #region Interfaces
    public async ValueTask<ErrorOr<IEnumerable<HallDto>>> Handle(SearchHallsQuery query, CancellationToken cancellationToken)
    {
        DateTime bookingEndDateTime = query.BookingStart.Add(query.BookingDuration);
        HallEntity[] entities = await thisDbContext.Halls.AsNoTracking()
            .Include(e => e.HallServices)
            .Include(e => e.Bookings)
            .Where(e => e.Capacity <= query.Capacity)
            .ToArrayAsync(cancellationToken);
        return entities.Where(
            e => !e.Bookings.Any(
                booking => booking.BookingStart < bookingEndDateTime && query.BookingStart < booking.BookingStart.Add(booking.BookingDuration)
            )
        ).ToDtos().ToErrorOr();
    }
    #endregion
}