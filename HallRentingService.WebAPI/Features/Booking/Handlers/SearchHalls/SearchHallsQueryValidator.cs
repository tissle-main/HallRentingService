using FluentValidation;

namespace HallRentingService.WebAPI.Features.Booking.Handlers.SearchHalls;

public sealed class SearchHallsQueryValidator : AbstractValidator<SearchHallsQuery>
{
    public SearchHallsQueryValidator()
    {
        base.RuleFor(q => q.Capacity).GreaterThan(0);
        base.RuleFor(q => q.BookingStart).GreaterThanOrEqualTo(DateTime.UtcNow);
        base.RuleFor(q => q.BookingDuration).GreaterThan(TimeSpan.Zero);
    }
}