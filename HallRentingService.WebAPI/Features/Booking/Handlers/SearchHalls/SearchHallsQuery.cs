using ErrorOr;
using Mediator;
using HallRentingService.WebAPI.Features.Halls.Dtos;

namespace HallRentingService.WebAPI.Features.Booking.Handlers.SearchHalls;

public sealed record class SearchHallsQuery(
    DateTime BookingStart,
    TimeSpan BookingDuration,
    int Capacity
) : IQuery<ErrorOr<IEnumerable<HallDto>>>;