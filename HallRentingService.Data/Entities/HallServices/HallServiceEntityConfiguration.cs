using Microsoft.EntityFrameworkCore;
using HallRentingService.Data.Shared.KeyedEntities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HallRentingService.Data.Entities.HallServices;

public sealed class HallServiceEntityConfiguration : IEntityTypeConfiguration<HallServiceEntity>
{
    #region Interfaces
    public void Configure(EntityTypeBuilder<HallServiceEntity> builder)
    {
        builder.ConfigureKeyedEntity();
        builder.Property(e => e.Name).IsRequired().HasMaxLength(HallServiceEntityConstants.NameMaxLength);
    }
    #endregion
}