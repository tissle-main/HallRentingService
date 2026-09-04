using Bogus;
using HallRentingService.Data;
using Microsoft.EntityFrameworkCore;
using HallRentingService.Data.Features.Booking;

namespace HallRentingService.WebAPI.Features.Booking.Dtos;

public static class BookingEntityDbSeeder
{
    extension(Faker<BookingEntity> thisFaker)
    {
        public async ValueTask<List<BookingEntity>> SeedDatabaseForHallAsync(AppDbContext db, CancellationToken cancellationToken, int min = 2, int max = 5)
        {
            List<BookingEntity> bookings = thisFaker.GenerateBetween(min, max);
            await db.Bookings.AddRangeAsync(bookings, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
            return bookings;
        }
        public async ValueTask<Dictionary<Guid, List<BookingEntity>>> SeedDatabaseForAllHallsAsync(
            AppDbContext db,
            CancellationToken cancellationToken,
            int min = 2,
            int max = 5
        )
        {
            Dictionary<Guid, List<BookingEntity>> dict = [];
            Guid[] ids = await db.Halls.AsNoTracking().Select(e => e.Id).ToArrayAsync(cancellationToken);
            foreach(Guid id in ids)
            {
                List<BookingEntity> bookings = await thisFaker.Clone().WithHallId(id).SeedDatabaseForHallAsync(db, cancellationToken, min, max);
                dict.Add(id, bookings);
            }
            return dict;
        }
    }
}