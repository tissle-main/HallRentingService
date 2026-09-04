using Microsoft.EntityFrameworkCore;
using HallRentingService.Data.Shared.KeyedEntities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HallRentingService.Data.Features.Booking;

public sealed class BookingEntityConfiguration : IEntityTypeConfiguration<BookingEntity>
{
    #region Interfaces
    public void Configure(EntityTypeBuilder<BookingEntity> builder)
    {
        builder.ConfigureKeyedEntity();
        builder.Property(e => e.BookingStart).IsRequired();
        builder.Property(e => e.BookingDuration).IsRequired();
        builder.Property(e => e.TotalPrice).IsRequired();
        builder.HasOne(b => b.Hall).WithMany(h => h.Bookings).HasForeignKey(b => b.HallId).IsRequired().OnDelete(DeleteBehavior.Restrict);
    }
    #endregion
}