using HallRentingService.Data.Shared.KeyedEntities;
using HallRentingService.Data.Features.Halls_HallServices;

namespace HallRentingService.Data.Features.HallServices;

public sealed class HallServiceEntity : IKeyedEntity
{
    //Value properties
    public Guid Id { get; set; } //Interfaces
    public HallServiceType ServiceType { get; set; }

    //Navigation properties
    public List<Hall_HallService_JoinEntity> Halls { get; set; } = [];
}