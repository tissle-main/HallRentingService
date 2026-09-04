using HallRentingService.Data.Features.Halls;
using HallRentingService.Data.Shared.KeyedEntities;

namespace HallRentingService.Data.Features.Booking;

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