namespace HallRentingService.WebAPI.Features.HallServices.Dtos;

public sealed class HallServiceDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}