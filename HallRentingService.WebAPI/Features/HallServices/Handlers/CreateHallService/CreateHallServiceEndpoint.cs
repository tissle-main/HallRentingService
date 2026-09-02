using ErrorOr;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using HallRentingService.WebAPI.Shared.Extensions;
using HallRentingService.WebAPI.Features.HallServices.Dtos;

namespace HallRentingService.WebAPI.Features.HallServices.Handlers.CreateHallService;

public static class CreateHallServiceEndpoint
{
    public const string Url = "/api/hall-service";

    public static async Task<IResult> CreateHallService(
        [FromBody] HallServiceDto dto,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        ErrorOr<Guid> response = await mediator.Send(new CreateHallServiceCommand(dto), cancellationToken);
        return response.ToHttpResult();
    }

    extension(RouteHandlerBuilder thisBuilder)
    {
        public RouteHandlerBuilder AddCreateHallServiceProductionProblems()
        {
            thisBuilder.ProducesProblem(StatusCodes.Status409Conflict);
            return thisBuilder.ProducesValidationProblem(StatusCodes.Status422UnprocessableEntity);
        }
    }
    extension(WebApplication thisApp)
    {
        public void AddCreateHallServiceEndpoint()
        {
            thisApp.MapPost(Url, CreateHallService)
                .WithName(nameof(CreateHallService))
                .Produces<Guid>(StatusCodes.Status200OK)
                .AddCreateHallServiceProductionProblems();
        }
    }
    extension(HttpClient thisHttpClient)
    {
        public async ValueTask<HttpResponseMessage> SendCreateHallServiceAsync(HallServiceDto dto, CancellationToken cancellationToken)
        {
            using HttpRequestMessage request = new(HttpMethod.Post, Url);
            request.Content = JsonContent.Create(dto);
            return await thisHttpClient.SendAsync(request, cancellationToken);
        }
    }
}