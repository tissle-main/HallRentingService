using ErrorOr;
using Mediator;
using HallRentingService.WebAPI.Features.HallServices.Dtos;

namespace HallRentingService.WebAPI.Features.HallServices.Handlers.GetHallServices;

public sealed record class GetHallServicesQuery(Guid[] Ids) : IQuery<ErrorOr<IEnumerable<HallServiceDto>>>;