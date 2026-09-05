namespace HallRentingService.WebAPI.Features.Booking.Services;

public interface IBookingPriceModifier
{
    public abstract decimal ApplyModifiers(decimal pricePerHour, DateTime bookingStart, TimeSpan bookingDuration);
}