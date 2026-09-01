using ErrorOr;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using HallRentingService.WebAPI.Shared.Extensions;
using HallRentingService.WebAPI.Shared.JoinEntities;

namespace HallRentingService.WebAPI.Features.Hall_HallServices.Handlers;

public static class UpdateHall_HallServices_JoinEntitiesEndpoint
{
    public const string Url = "/api/hall-hallservices";

    public static async Task<IResult> UpdateHall_HallServices(
        [FromBody] UpdateHall_HallServices_JoinEntitiesCommand command,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        ErrorOr<Unit> response = await mediator.Send(command, cancellationToken);
        return response.ToHttpResult();
    }

    extension(RouteHandlerBuilder thisBuilder)
    {
        public RouteHandlerBuilder AddUpdateHall_HallServicesProductionProblems()
        {
            return thisBuilder.AddUpdateJoinEntitiesProductionProblems();
        }
    }
    extension(WebApplication thisApp)
    {
        public void AddUpdateHall_HallServicesEndpoint()
        {
            thisApp.MapPut(Url, UpdateHall_HallServices)
                .WithName(nameof(UpdateHall_HallServices))
                .Produces(StatusCodes.Status204NoContent)
                .AddUpdateHall_HallServicesProductionProblems();
        }
    }
    extension(HttpClient thisHttpClient)
    {
        public async ValueTask<HttpResponseMessage> SendUpdateHall_HallServicesAsync(
            UpdateHall_HallServices_JoinEntitiesCommand command,
            CancellationToken cancellationToken
        )
        {
            using HttpRequestMessage request = new(HttpMethod.Put, Url);
            request.Content = JsonContent.Create(command);
            return await thisHttpClient.SendAsync(request, cancellationToken);
        }
    }
}