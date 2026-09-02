namespace HallRentingService.WebAPI.Features.Halls_HallServices;

public sealed class Hall_JoinDto
{
    public Guid HallId { get; set; }
    public float Price { get; set; }
}