using HallRentingService.WebAPI.Features.Hall_HallServices.Dtos;

namespace HallRentingService.WebAPI.Features.Halls.Dtos;

public sealed class HallDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public int Capacity { get; set; }
    public int BasePrice { get; set; }
    public List<HallService_JoinDto> HallServices { get; set; } = [];
    public List<Guid> Bookings { get; set; } = [];
}