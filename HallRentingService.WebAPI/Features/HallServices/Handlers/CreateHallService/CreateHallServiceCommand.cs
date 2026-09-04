using ErrorOr;
using Mediator;
using HallRentingService.WebAPI.Features.HallServices.Dtos;
using HallRentingService.WebAPI.Shared.Behaviors.DbTransaction;

namespace HallRentingService.WebAPI.Features.HallServices.Handlers.CreateHallService;

public sealed record class CreateHallServiceCommand(HallServiceDto HallService) : IDbTransactionBehaviorMessage, ICommand<ErrorOr<Guid>>;