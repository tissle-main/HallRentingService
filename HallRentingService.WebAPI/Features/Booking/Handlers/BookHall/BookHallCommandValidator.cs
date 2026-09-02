using FluentValidation;

namespace HallRentingService.WebAPI.Features.Booking.Handlers.BookHall;

public sealed class BookHallCommandValidator : AbstractValidator<BookHallCommand>
{
    public BookHallCommandValidator()
    {
        base.RuleFor(c => c.BookingStart).GreaterThanOrEqualTo(DateTime.UtcNow);
        base.RuleFor(c => c.BookingDuration).GreaterThan(TimeSpan.Zero);
    }
}