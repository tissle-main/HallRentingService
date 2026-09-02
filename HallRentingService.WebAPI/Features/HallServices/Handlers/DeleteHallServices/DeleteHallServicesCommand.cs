using ErrorOr;
using Mediator;

namespace HallRentingService.WebAPI.Features.HallServices.Handlers.DeleteHallServices;

public sealed record class DeleteHallServicesCommand(Guid[] Ids) : ICommand<ErrorOr<Unit>>;
