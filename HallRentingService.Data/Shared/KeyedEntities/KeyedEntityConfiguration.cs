using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HallRentingService.Data.Shared.KeyedEntities;

public static class KeyedEntityConfiguration
{
    extension<TEntity>(EntityTypeBuilder<TEntity> thisBuilder) where TEntity : class, IKeyedEntity
    {
        public void ConfigureKeyedEntity()
        {
            thisBuilder.HasKey(e => e.Id);
        }
    }
}