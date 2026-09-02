using ErrorOr;
using Mediator;
using HallRentingService.WebAPI.Shared.Behaviors.DbTransaction;

namespace HallRentingService.WebAPI.Features.Booking.Handlers.BookHall;

public sealed record class BookHallCommand(
    Guid HallId,
    DateTime BookingStart,
    TimeSpan BookingDuration,
    List<Guid> HallServices
) : IDbTransactionBehaviorMessage, ICommand<ErrorOr<BookHallResponse>>;