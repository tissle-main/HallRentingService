using Bogus;
using HallRentingService.Data;
using Microsoft.EntityFrameworkCore;
using HallRentingService.Data.Features.Halls;
using HallRentingService.Data.Features.HallServices;
using HallRentingService.Data.Features.Halls_HallServices;

namespace HallRentingService.WebAPI.Features.Halls_HallServices;

public static class Hall_HallService_JoinEntityDbSeeder
{
    extension(Faker<Hall_HallService_JoinEntity> thisFaker)
    {
        public async ValueTask<List<Hall_HallService_JoinEntity>> SeedDatabaseAsync(AppDbContext db, CancellationToken cancellationToken)
        {
            HallEntity[] halls = await db.Halls.AsNoTracking().ToArrayAsync(cancellationToken);
            HallServiceEntity[] hallServices = await db.HallServices.AsNoTracking().ToArrayAsync(cancellationToken);
            List<Hall_HallService_JoinEntity> jes = halls.SelectMany(hall =>
            {
                return hallServices.Select(hs =>
                {
                    return thisFaker.Clone().WithHallId(hall.Id).WithHallServiceId(hs.Id).Generate();
                });
            }).ToList();
            await db.Hall_HallServices.AddRangeAsync(jes, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
            return jes;
        }
    }
}