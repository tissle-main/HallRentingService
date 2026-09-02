using Microsoft.EntityFrameworkCore;
using HallRentingService.Data.Entities.Halls;
using HallRentingService.Data.Entities.Booking;
using HallRentingService.Data.Shared.KeyedEntities;
using HallRentingService.Data.Entities.HallServices;
using HallRentingService.Data.Entities.Halls_HallServices;

namespace HallRentingService.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    #region Instance
    public DbSet<HallEntity> Halls { get; set; } = null!; //Init by EF Core
    public DbSet<HallServiceEntity> HallServices { get; set; } = null!; //Init by EF Core
    public DbSet<Hall_HallService_JoinEntity> Hall_HallServices { get; set; } = null!; //Init by EF Core
    public DbSet<BookingEntity> Bookings { get; set; } = null!; //Init by EF Core
    #endregion

    #region Base
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        this.GenerateIdForKeyedEntities();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }
    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        this.GenerateIdForKeyedEntities();
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
    #endregion
}