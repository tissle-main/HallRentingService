using ErrorOr;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using HallRentingService.WebAPI.Shared.Extensions;
using HallRentingService.WebAPI.Features.Halls.Dtos;

namespace HallRentingService.WebAPI.Features.Halls.Handlers.GetHalls;

public static class GetHallsEndpoint
{
    public const string Url = "/api/halls";

    public static string CreateSendableUrl(Guid[] ids)
    {
        IEnumerable<KeyValuePair<string, string?>> queryParams = ids.Select(id =>
        {
            return new KeyValuePair<string, string?>(nameof(ids), id.ToString());
        });
        return QueryHelpers.AddQueryString(Url, queryParams);
    }
    public static async Task<IResult> GetHalls(
        [FromQuery] Guid[]? ids,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        ErrorOr<IEnumerable<HallDto>> response = await mediator.Send(new GetHallsQuery(ids ?? []), cancellationToken);
        return response.ToHttpResult();
    }

    extension(RouteHandlerBuilder thisBuilder)
    {
        public RouteHandlerBuilder AddGetHallsProductionProblems()
        {
            return thisBuilder.ProducesProblem(StatusCodes.Status404NotFound);
        }
    }
    extension(WebApplication thisApp)
    {
        public void AddGetHallsEndpoint()
        {
            thisApp.MapGet(Url, GetHalls)
                .WithName(nameof(GetHalls))
                .Produces<IEnumerable<HallDto>>(StatusCodes.Status200OK)
                .AddGetHallsProductionProblems();
        }
    }
    extension(HttpClient thisHttpClient)
    {
        public async ValueTask<HttpResponseMessage> SendGetHallsAsync(Guid[] ids, CancellationToken cancellationToken)
        {
            using HttpRequestMessage request = new(HttpMethod.Get, CreateSendableUrl(ids));
            return await thisHttpClient.SendAsync(request, cancellationToken);
        }
    }
}