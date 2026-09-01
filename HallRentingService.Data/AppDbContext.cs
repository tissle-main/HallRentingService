using Microsoft.EntityFrameworkCore;
using PhoenixKC.Data.Shared.KeyedEntities;

namespace HallRentingService.Data;

public sealed class AppDbContext : DbContext
{
    #region Instance
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        base.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }
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
        int changedNumber = base.SaveChanges(acceptAllChangesOnSuccess);
        base.ChangeTracker.Clear();
        return changedNumber;
    }
    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        this.GenerateIdForKeyedEntities();
        int changedNumber = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        base.ChangeTracker.Clear();
        return changedNumber;
    }
    #endregion
}