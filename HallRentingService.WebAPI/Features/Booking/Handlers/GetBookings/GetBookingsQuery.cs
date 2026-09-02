using ErrorOr;
using Mediator;
using HallRentingService.WebAPI.Features.Booking.Dtos;

namespace HallRentingService.WebAPI.Features.Booking.Handlers.GetBookings;

public sealed record class GetBookingsQuery(Guid[] Ids) : IQuery<ErrorOr<IEnumerable<BookingDto>>>;