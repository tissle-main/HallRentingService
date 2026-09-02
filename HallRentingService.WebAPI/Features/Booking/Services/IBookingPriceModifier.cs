namespace HallRentingService.WebAPI.Features.Booking.Services;

public interface IBookingPriceModifier
{
    public abstract float ApplyModifiers(float pricePerHour, DateTime bookingStart, TimeSpan bookingDuration);
}