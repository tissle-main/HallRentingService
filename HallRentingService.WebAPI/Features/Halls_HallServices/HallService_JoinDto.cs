namespace HallRentingService.WebAPI.Features.Halls_HallServices;

public sealed class HallService_JoinDto
{
    public Guid HallServiceId { get; set; }
    public decimal Price { get; set; }
}