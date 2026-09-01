using Microsoft.EntityFrameworkCore;
using HallRentingService.Data.Entities.Halls;
using HallRentingService.Data.Shared.JoinEntities;
using HallRentingService.Data.Entities.HallServices;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HallRentingService.Data.Entities.Halls_HallServices;

public sealed class Hall_HallService_JoinEntityConfiguration : IEntityTypeConfiguration<Hall_HallService_JoinEntity>
{
    #region Interfaces
    public void Configure(EntityTypeBuilder<Hall_HallService_JoinEntity> builder)
    {
        builder.ConfigureJoinEntity<Hall_HallService_JoinEntity, HallEntity, HallServiceEntity>(hall => hall.HallServices, services => services.Halls);
        builder.Property(je => je.Price).IsRequired();
    }
    #endregion
}