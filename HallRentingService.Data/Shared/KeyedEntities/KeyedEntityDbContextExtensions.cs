using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HallRentingService.Data.Shared.KeyedEntities;

public static class KeyedEntityDbContextExtensions
{
    extension(DbContext thisDbContext)
    {
        public void GenerateIdForKeyedEntities()
        {
            foreach(EntityEntry<IKeyedEntity> entry in thisDbContext.ChangeTracker.Entries<IKeyedEntity>())
            {
                if(entry.State is EntityState.Added)
                {
                    entry.Entity.Id = Guid.CreateVersion7();
                }
            }
        }
    }
}