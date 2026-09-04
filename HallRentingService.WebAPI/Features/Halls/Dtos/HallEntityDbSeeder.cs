using Bogus;
using HallRentingService.Data;
using HallRentingService.Data.Features.Halls;

namespace HallRentingService.WebAPI.Features.Halls.Dtos;

public static class HallEntityDbSeeder
{
    extension(Faker<HallEntity> thisFaker)
    {
        public async ValueTask<List<HallEntity>> SeedDatabaseAsync(AppDbContext db, CancellationToken cancellationToken, int min = 2, int max = 5)
        {
            List<HallEntity> halls = thisFaker.GenerateBetween(min, max);
            await db.Halls.AddRangeAsync(halls, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
            return halls;
        }
    }
}