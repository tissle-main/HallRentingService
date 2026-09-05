using Bogus;
using HallRentingService.Data.Features.Booking;

namespace HallRentingService.WebAPI.Features.Booking.Dtos;

public static class BookingEntityFaker
{
    extension(Faker<BookingEntity> thisFaker)
    {
        public Faker<BookingEntity> Valid()
        {
            return thisFaker.CustomInstantiator(g => new BookingEntity()
            {
                BookingStart = DateTime.UtcNow.AddDays(g.Random.Number(1, 5)),
                BookingDuration = TimeSpan.FromHours(g.Random.Number(1, 10)),
                TotalPrice = g.Random.Decimal(5000, 50000)
            });
        }
        public Faker<BookingEntity> WithBookingStart(DateTime bookingStart)
        {
            return thisFaker.RuleFor(e => e.BookingStart, bookingStart);
        }
        public Faker<BookingEntity> WithBookingDuration(TimeSpan bookingDuration)
        {
            return thisFaker.RuleFor(e => e.BookingDuration, bookingDuration);
        }
        public Faker<BookingEntity> WithHallId(Guid hallId)
        {
            return thisFaker.RuleFor(e => e.HallId, hallId);
        }
    }
}