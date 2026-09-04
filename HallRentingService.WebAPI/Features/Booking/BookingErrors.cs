using ErrorOr;

namespace HallRentingService.WebAPI.Features.Booking;

public static class BookingErrors
{
    private static string Code
    {
        get => field ??= nameof(BookingErrors)[..^"Errors".Length];
    }

    public static Error IdsNotFound(Guid[] ids)
    {
        string description = $"Some bookings not found. Missing ids: [{string.Join(", ", ids)}].";
        return Error.NotFound($"{Code}.{nameof(IdsNotFound)}", description);
    }
    public static Error BookingOverlaps(DateTime bookingStart, TimeSpan bookingDuration)
    {
        string description = $"Booking period '{bookingStart}' - '{bookingStart.Add(bookingDuration)}' overlaps with other bookings.";
        return Error.Conflict($"{Code}.{nameof(BookingOverlaps)}", description);
    }
}