using HallRentingService.WebAPI.Features.Halls_HallServices;

namespace HallRentingService.WebAPI.Features.Halls.Dtos;

public sealed class HallDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public int Capacity { get; set; }
    public decimal PricePerHour { get; set; }
    public List<HallService_JoinDto> HallServices { get; set; } = [];
    public List<Guid> Bookings { get; set; } = [];
}