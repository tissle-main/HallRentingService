using HallRentingService.WebAPI.Features.Booking.Services;
using HallRentingService.WebAPI.Features.Booking.Handlers.BookHall;
using HallRentingService.WebAPI.Features.Booking.Handlers.GetBookings;
using HallRentingService.WebAPI.Features.Booking.Handlers.SearchHalls;

namespace HallRentingService.WebAPI.Features.Booking;

public sealed class BookingFeatureProvider : FeatureProvider
{
    #region Base
    public override void AddServices(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IBookingPriceModifier, BookingPriceModifier>();
    }
    public override void UseMiddleware(WebApplication app)
    {
        app.AddSearchHallsEndpoint();
        app.AddBookHallEndpoint();
        app.AddGetBookingsEndpoint();
    }
    #endregion
}