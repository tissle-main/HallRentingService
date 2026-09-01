using ErrorOr;
using Mediator;
using HallRentingService.WebAPI.Features.Halls.Dtos;
using HallRentingService.WebAPI.Shared.Behaviors.DbTransaction;

namespace HallRentingService.WebAPI.Features.Halls.Handlers.UpdateHall;

public sealed record class UpdateHallCommand(HallDto Hall) : IDbTransactionBehaviorMessage, ICommand<ErrorOr<Unit>>;