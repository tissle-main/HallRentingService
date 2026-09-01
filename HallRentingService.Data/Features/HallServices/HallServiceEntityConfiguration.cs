using Microsoft.EntityFrameworkCore;
using HallRentingService.Data.Shared.KeyedEntities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HallRentingService.Data.Features.HallServices;

public sealed class HallServiceEntityConfiguration : IEntityTypeConfiguration<HallServiceEntity>
{
    #region Interfaces
    public void Configure(EntityTypeBuilder<HallServiceEntity> builder)
    {
        builder.ConfigureKeyedEntity();
        builder.Property(e => e.ServiceType).IsRequired();
    }
    #endregion
}