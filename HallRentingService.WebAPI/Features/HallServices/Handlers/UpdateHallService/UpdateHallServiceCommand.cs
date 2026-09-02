using ErrorOr;
using Mediator;
using HallRentingService.WebAPI.Shared.Behaviors.DbTransaction;
using HallRentingService.WebAPI.Features.HallServices.Dtos;

namespace HallRentingService.WebAPI.Features.HallServices.Handlers.UpdateHallService;

public sealed record class UpdateHallServiceCommand(HallServiceDto HallService) : IDbTransactionBehaviorMessage, ICommand<ErrorOr<Unit>>;
