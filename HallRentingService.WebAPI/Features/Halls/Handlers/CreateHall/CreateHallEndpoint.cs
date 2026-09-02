using ErrorOr;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using HallRentingService.WebAPI.Shared.Extensions;
using HallRentingService.WebAPI.Features.Halls.Dtos;

namespace HallRentingService.WebAPI.Features.Halls.Handlers.CreateHall;

public static class CreateHallEndpoint
{
    public const string Url = "/api/hall";

    public static async Task<IResult> CreateHall(
        [FromBody] HallDto dto,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        ErrorOr<Guid> response = await mediator.Send(new CreateHallCommand(dto), cancellationToken);
        return response.ToHttpResult();
    }

    extension(RouteHandlerBuilder thisBuilder)
    {
        public RouteHandlerBuilder AddCreateHallProductionProblems()
        {
            return thisBuilder.ProducesValidationProblem(StatusCodes.Status422UnprocessableEntity);
        }
    }
    extension(WebApplication thisApp)
    {
        public void AddCreateHallEndpoint()
        {
            thisApp.MapPost(Url, CreateHall)
                .WithName(nameof(CreateHall))
                .Produces<Guid>(StatusCodes.Status200OK)
                .AddCreateHallProductionProblems();
        }
    }
    extension(HttpClient thisHttpClient)
    {
        public async ValueTask<HttpResponseMessage> SendCreateHallAsync(HallDto dto, CancellationToken cancellationToken)
        {
            using HttpRequestMessage request = new(HttpMethod.Post, Url);
            request.Content = JsonContent.Create(dto);
            return await thisHttpClient.SendAsync(request, cancellationToken);
        }
    }
}