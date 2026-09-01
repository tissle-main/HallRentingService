using ErrorOr;
using Mediator;
using HallRentingService.Data;
using HallRentingService.Data.Entities.Halls;
using HallRentingService.Data.Entities.HallServices;
using HallRentingService.WebAPI.Shared.JoinEntities;
using HallRentingService.Data.Entities.Halls_HallServices;

namespace HallRentingService.WebAPI.Features.Hall_HallServices.Handlers;

public sealed class UpdateHall_HallServices_JoinEntitiesHandler(
    AppDbContext thisDbContext
) : UpdateJoinEntitiesHandler<UpdateHall_HallServices_JoinEntitiesCommand, Hall_HallService_JoinEntity, HallEntity, HallServiceEntity>(
        thisDbContext,
        leftIds => Error.NotFound(),
        rightIds => Error.NotFound()
    ),
    ICommandHandler<UpdateHall_HallServices_JoinEntitiesCommand, ErrorOr<Unit>>;