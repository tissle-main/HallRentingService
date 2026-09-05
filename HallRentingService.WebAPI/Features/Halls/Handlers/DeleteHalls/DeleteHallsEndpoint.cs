using ErrorOr;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using HallRentingService.WebAPI.Shared.Extensions;

namespace HallRentingService.WebAPI.Features.Halls.Handlers.DeleteHalls;

public static class DeleteHallsEndpoint
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
    public static async Task<IResult> DeleteHalls(
        [FromQuery] Guid[]? ids,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        ErrorOr<Unit> response = await mediator.Send(new DeleteHallsCommand(ids ?? []), cancellationToken);
        return response.ToHttpResult();
    }

    extension(RouteHandlerBuilder thisBuilder)
    {
        public RouteHandlerBuilder AddDeleteHallsProductionProblems()
        {
            return thisBuilder.ProducesProblem(StatusCodes.Status404NotFound);
        }
    }
    extension(WebApplication thisApp)
    {
        public void AddDeleteHallsEndpoint()
        {
            thisApp.MapDelete(Url, DeleteHalls)
                .WithName(nameof(DeleteHalls))
                .Produces(StatusCodes.Status204NoContent)
                .AddDeleteHallsProductionProblems();
        }
    }
    extension(HttpClient thisHttpClient)
    {
        public async ValueTask<HttpResponseMessage> SendDeleteHallsAsync(Guid[] ids, CancellationToken cancellationToken)
        {
            using HttpRequestMessage request = new(HttpMethod.Delete, CreateSendableUrl(ids));
            return await thisHttpClient.SendAsync(request, cancellationToken);
        }
    }
}