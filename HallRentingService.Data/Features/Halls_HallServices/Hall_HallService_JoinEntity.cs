using HallRentingService.Data.Features.Halls;
using HallRentingService.Data.Features.HallServices;

namespace HallRentingService.Data.Features.Halls_HallServices;

public sealed class Hall_HallService_JoinEntity
{
    //Value properties
    public Guid HallId { get; set; }
    public Guid HallServiceId { get; set; }
    public decimal Price { get; set; }

    //Navigation properties
    public HallEntity? Hall { get; set; }
    public HallServiceEntity? HallService { get; set; }
}