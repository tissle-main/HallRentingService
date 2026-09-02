using ErrorOr;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using HallRentingService.WebAPI.Shared.Extensions;
using HallRentingService.WebAPI.Features.HallServices.Dtos;

namespace HallRentingService.WebAPI.Features.HallServices.Handlers.UpdateHallService;

public static class UpdateHallServiceEndpoint
{
    public const string Url = "/api/hall-service";

    public static async Task<IResult> UpdateHallService(
        [FromBody] HallServiceDto dto,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        ErrorOr<Unit> response = await mediator.Send(new UpdateHallServiceCommand(dto), cancellationToken);
        return response.ToHttpResult();
    }

    extension(RouteHandlerBuilder thisBuilder)
    {
        public RouteHandlerBuilder AddUpdateHallServiceProductionProblems()
        {
            thisBuilder.ProducesProblem(StatusCodes.Status409Conflict);
            return thisBuilder.ProducesValidationProblem(StatusCodes.Status422UnprocessableEntity);
        }
    }
    extension(WebApplication thisApp)
    {
        public void AddUpdateHallServiceEndpoint()
        {
            thisApp.MapPut(Url, UpdateHallService)
                .WithName(nameof(UpdateHallService))
                .Produces(StatusCodes.Status204NoContent)
                .AddUpdateHallServiceProductionProblems();
        }
    }
    extension(HttpClient thisHttpClient)
    {
        public async ValueTask<HttpResponseMessage> SendUpdateHallServiceAsync(HallServiceDto dto, CancellationToken cancellationToken)
        {
            using HttpRequestMessage request = new(HttpMethod.Put, Url);
            request.Content = JsonContent.Create(dto);
            return await thisHttpClient.SendAsync(request, cancellationToken);
        }
    }
}