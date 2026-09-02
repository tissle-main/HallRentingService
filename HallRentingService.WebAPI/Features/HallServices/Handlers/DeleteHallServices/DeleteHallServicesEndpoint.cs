using ErrorOr;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using HallRentingService.WebAPI.Shared.Extensions;

namespace HallRentingService.WebAPI.Features.HallServices.Handlers.DeleteHallServices;

public static class DeleteHallServicesEndpoint
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

    public static async Task<IResult> DeleteHallServices(
        [FromQuery] Guid[] ids,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        ErrorOr<Unit> response = await mediator.Send(new DeleteHallServicesCommand(ids), cancellationToken);
        return response.ToHttpResult();
    }

    extension(RouteHandlerBuilder thisBuilder)
    {
        public RouteHandlerBuilder AddDeleteHallServicesProductionProblems()
        {
            return thisBuilder.ProducesProblem(StatusCodes.Status404NotFound);
        }
    }
    extension(WebApplication thisApp)
    {
        public void AddDeleteHallServicesEndpoint()
        {
            thisApp.MapDelete(Url, DeleteHallServices)
                .WithName(nameof(DeleteHallServices))
                .Produces(StatusCodes.Status204NoContent)
                .AddDeleteHallServicesProductionProblems();
        }
    }
    extension(HttpClient thisHttpClient)
    {
        public async ValueTask<HttpResponseMessage> SendDeleteHallServicesAsync(Guid[] ids, CancellationToken cancellationToken)
        {
            using HttpRequestMessage request = new(HttpMethod.Delete, CreateSendableUrl(ids));
            return await thisHttpClient.SendAsync(request, cancellationToken);
        }
    }
}
