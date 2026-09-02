using Bogus;

namespace HallRentingService.WebAPI.Features.Booking.Handlers.BookHall;

public static class BookHallCommandFaker
{
    extension(Faker<BookHallCommand> thisFaker)
    {
        public Faker<BookHallCommand> Valid()
        {
            return thisFaker.CustomInstantiator(g =>
            {
                Guid id = Guid.CreateVersion7();
                DateTime bookingStart = DateTime.UtcNow.AddDays(g.Random.Number(1, 5));
                TimeSpan bookingDuration = TimeSpan.FromHours(g.Random.Number(1, 10));
                return new BookHallCommand(id, bookingStart, bookingDuration, []);
            });
        }
        public Faker<BookHallCommand> WithExpiredBookingStart()
        {
            return thisFaker.RuleFor(e => e.BookingStart, DateTime.UtcNow.AddDays(-1));
        }
        public Faker<BookHallCommand> WithZeroBookingDuration()
        {
            return thisFaker.RuleFor(e => e.BookingDuration, TimeSpan.Zero);
        }
        public Faker<BookHallCommand> WithNegativeBookingDuration()
        {
            return thisFaker.RuleFor(e => e.BookingDuration, TimeSpan.FromHours(-1));
        }
    }
}