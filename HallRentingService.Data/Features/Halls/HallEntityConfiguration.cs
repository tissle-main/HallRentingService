using Microsoft.EntityFrameworkCore;
using HallRentingService.Data.Shared.KeyedEntities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HallRentingService.Data.Features.Halls;

public sealed class HallEntityConfiguration : IEntityTypeConfiguration<HallEntity>
{
    #region Interfaces
    public void Configure(EntityTypeBuilder<HallEntity> builder)
    {
        builder.ConfigureKeyedEntity();
        builder.Property(e => e.Name).IsRequired().HasMaxLength(HallEntityConstants.NameMaxLength);
        builder.Property(e => e.Capacity).IsRequired();
        builder.Property(e => e.PricePerHour).IsRequired().HasPrecision(18, 2);
    }
    #endregion
}