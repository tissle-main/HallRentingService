using HallRentingService.Data.Entities.Halls;
using HallRentingService.Data.Entities.HallServices;

namespace HallRentingService.Data.Entities.Halls_HallServices;

public sealed class Hall_HallService_JoinEntity
{
    //Value properties
    public Guid HallId { get; set; }
    public Guid HallServiceId { get; set; }
    public float Price { get; set; }

    //Navigation properties
    public HallEntity? Hall { get; set; }
    public HallServiceEntity? HallService { get; set; }
}