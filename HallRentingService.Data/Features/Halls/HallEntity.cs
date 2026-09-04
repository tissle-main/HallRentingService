using HallRentingService.Data.Features.Booking;
using HallRentingService.Data.Shared.KeyedEntities;
using HallRentingService.Data.Features.Halls_HallServices;

namespace HallRentingService.Data.Features.Halls;

public sealed class HallEntity : IKeyedEntity
{
    //Value properties
    public Guid Id { get; set; } //Interfaces
    public required string Name { get; set; }
    public int Capacity { get; set; }
    public float PricePerHour { get; set; }

    //Navigation properties
    public List<Hall_HallService_JoinEntity> HallServices { get; set; } = [];
    public List<BookingEntity> Bookings { get; set; } = [];
}