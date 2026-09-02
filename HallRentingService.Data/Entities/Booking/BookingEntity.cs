using HallRentingService.Data.Entities.Halls;
using HallRentingService.Data.Shared.KeyedEntities;

namespace HallRentingService.Data.Entities.Booking;

public sealed class BookingEntity : IKeyedEntity
{
    //Value properties
    public Guid Id { get; set; } //Interfaces
    public DateTime BookingStart { get; set; }
    public TimeSpan BookingDuration { get; set; }
    public float TotalPrice { get; set; }
    public Guid HallId { get; set; }

    //Navigation properties
    public HallEntity? Hall { get; set; }
}