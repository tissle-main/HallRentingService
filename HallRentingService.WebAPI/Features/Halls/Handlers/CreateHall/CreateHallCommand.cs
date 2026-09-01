using ErrorOr;
using Mediator;
using HallRentingService.WebAPI.Features.Halls.Dtos;
using HallRentingService.WebAPI.Shared.Behaviors.DbTransaction;

namespace HallRentingService.WebAPI.Features.Halls.Handlers.CreateHall;

public sealed record class CreateHallCommand(HallDto Hall) : IDbTransactionBehaviorMessage, ICommand<ErrorOr<Guid>>;