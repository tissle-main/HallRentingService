using Bogus;

namespace HallRentingService.WebAPI.Features.Booking.Handlers.SearchHalls;

public static class SearchHallsQueryFaker
{
    extension(Faker<SearchHallsQuery> thisFaker)
    {
        public Faker<SearchHallsQuery> Valid()
        {
            return thisFaker.CustomInstantiator(g =>
            {
                DateTime bookingStart = DateTime.UtcNow.AddDays(g.Random.Number(1, 5));
                TimeSpan bookingDuration = TimeSpan.FromHours(g.Random.Number(1, 10));
                int capacity = g.Random.Number(1, 100);
                return new SearchHallsQuery(bookingStart, bookingDuration, capacity);
            });
        }
        public Faker<SearchHallsQuery> WithBookingStart(DateTime bookingStart)
        {
            return thisFaker.RuleFor(e => e.BookingStart, bookingStart);
        }
        public Faker<SearchHallsQuery> WithBookingDuration(TimeSpan bookingDuration)
        {
            return thisFaker.RuleFor(e => e.BookingDuration, bookingDuration);
        }
        public Faker<SearchHallsQuery> WithCapacity(int capacity)
        {
            return thisFaker.RuleFor(e => e.Capacity, capacity);
        }
        public Faker<SearchHallsQuery> WithExpiredBookingStart()
        {
            return thisFaker.RuleFor(e => e.BookingStart, DateTime.UtcNow.AddDays(-1));
        }
        public Faker<SearchHallsQuery> WithZeroBookingDuration()
        {
            return thisFaker.RuleFor(e => e.BookingDuration, TimeSpan.Zero);
        }
        public Faker<SearchHallsQuery> WithNegativeBookingDuration()
        {
            return thisFaker.RuleFor(e => e.BookingDuration, TimeSpan.FromHours(-1));
        }
        public Faker<SearchHallsQuery> WithZeroCapacity()
        {
            return thisFaker.RuleFor(e => e.Capacity, 0);
        }
        public Faker<SearchHallsQuery> WithNegativeCapacity()
        {
            return thisFaker.RuleFor(e => e.Capacity, -1);
        }
    }
}