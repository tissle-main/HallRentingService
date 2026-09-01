using ErrorOr;
using Mediator;
using HallRentingService.WebAPI.Features.Halls.Dtos;

namespace HallRentingService.WebAPI.Features.Halls.Handlers.GetHalls;

public sealed record class GetHallsQuery(Guid[] Ids) : IQuery<ErrorOr<IEnumerable<HallDto>>>;