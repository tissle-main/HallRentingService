using ErrorOr;
using Mediator;
using HallRentingService.Data.Entities.Halls;
using HallRentingService.Data.Entities.HallServices;
using HallRentingService.WebAPI.Shared.JoinEntities;
using HallRentingService.Data.Entities.Halls_HallServices;

namespace HallRentingService.WebAPI.Features.Hall_HallServices.Handlers;

public sealed record class UpdateHall_HallServices_JoinEntitiesCommand(
    IReadOnlyCollection<Hall_HallService_JoinEntity> OldEntities,
    IReadOnlyCollection<Hall_HallService_JoinEntity> NewEntities
) : IUpdateJoinEntitiesMessage<Hall_HallService_JoinEntity, HallEntity, HallServiceEntity>, ICommand<ErrorOr<Unit>>;