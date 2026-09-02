using ErrorOr;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using HallRentingService.WebAPI.Shared.Extensions;
using HallRentingService.WebAPI.Features.HallServices.Dtos;

namespace HallRentingService.WebAPI.Features.HallServices.Handlers.GetHallServices;

public static class GetHallServicesEndpoint
{
    public const string Url = "/api/hall-services";

    public static string CreateSendableUrl(Guid[] ids)
    {
        IEnumerable<KeyValuePair<string, string?>> queryParams = ids.Select(id =>
        {
            return new KeyValuePair<string, string?>(nameof(ids), id.ToString());
        });
        return QueryHelpers.AddQueryString(Url, queryParams);
    }
    public static async Task<IResult> GetHallServices(
        [FromQuery] Guid[] ids,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        ErrorOr<IEnumerable<HallServiceDto>> response = await mediator.Send(new GetHallServicesQuery(ids), cancellationToken);
        return response.ToHttpResult();
    }

    extension(RouteHandlerBuilder thisBuilder)
    {
        public RouteHandlerBuilder AddGetHallServicesProductionProblems()
        {
            return thisBuilder.ProducesProblem(StatusCodes.Status404NotFound);
        }
    }
    extension(WebApplication thisApp)
    {
        public void AddGetHallServicesEndpoint()
        {
            thisApp.MapGet(Url, GetHallServices)
                .WithName(nameof(GetHallServices))
                .Produces<IEnumerable<HallServiceDto>>(StatusCodes.Status200OK)
                .AddGetHallServicesProductionProblems();
        }
    }
    extension(HttpClient thisHttpClient)
    {
        public async ValueTask<HttpResponseMessage> SendGetHallServicesAsync(Guid[] ids, CancellationToken cancellationToken)
        {
            using HttpRequestMessage request = new(HttpMethod.Get, CreateSendableUrl(ids));
            return await thisHttpClient.SendAsync(request, cancellationToken);
        }
    }
}