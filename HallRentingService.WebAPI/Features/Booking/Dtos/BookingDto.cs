namespace HallRentingService.WebAPI.Features.Booking.Dtos;

public sealed class BookingDto
{
    public Guid Id { get; set; }
    public DateTime BookingStart { get; set; }
    public TimeSpan BookingDuration { get; set; }
    public float TotalPrice { get; set; }
    public Guid HallId { get; set; }
}