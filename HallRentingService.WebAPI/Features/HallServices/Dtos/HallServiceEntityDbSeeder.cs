using Bogus;
using HallRentingService.Data;
using HallRentingService.Data.Features.HallServices;

namespace HallRentingService.WebAPI.Features.HallServices.Dtos;

public static class HallServiceEntityDbSeeder
{
    extension(Faker<HallServiceEntity> thisFaker)
    {
        public async ValueTask<List<HallServiceEntity>> SeedDatabaseAsync(AppDbContext db, CancellationToken cancellationToken, int min = 2, int max = 5)
        {
            List<HallServiceEntity> hallServices = thisFaker.GenerateBetween(min, max);
            await db.HallServices.AddRangeAsync(hallServices, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
            return hallServices;
        }
    }
}