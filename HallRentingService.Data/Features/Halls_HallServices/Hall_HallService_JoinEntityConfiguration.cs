using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HallRentingService.Data.Features.Halls_HallServices;

public sealed class Hall_HallService_JoinEntityConfiguration : IEntityTypeConfiguration<Hall_HallService_JoinEntity>
{
    #region Interfaces
    public void Configure(EntityTypeBuilder<Hall_HallService_JoinEntity> builder)
    {
        builder.HasKey(je => new { je.HallId, je.HallServiceId });
        builder.HasOne(je => je.Hall).WithMany(h => h.HallServices).HasForeignKey(je => je.HallId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(je => je.HallService).WithMany(hs => hs.Halls).HasForeignKey(je => je.HallServiceId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.Property(je => je.Price).IsRequired();
    }
    #endregion
}