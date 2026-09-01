using ErrorOr;
using Mediator;

namespace HallRentingService.WebAPI.Features.Halls.Handlers.DeleteHalls;

public sealed record class DeleteHallsCommand(Guid[] Ids) : ICommand<ErrorOr<Unit>>;