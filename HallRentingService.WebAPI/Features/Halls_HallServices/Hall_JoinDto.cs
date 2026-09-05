namespace HallRentingService.WebAPI.Features.Halls_HallServices;

public sealed class Hall_JoinDto
{
    public Guid HallId { get; set; }
    public decimal Price { get; set; }
}