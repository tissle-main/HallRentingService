using ErrorOr;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using HallRentingService.WebAPI.Shared.Extensions;
using HallRentingService.WebAPI.Features.Halls.Dtos;

namespace HallRentingService.WebAPI.Features.Booking.Handlers.SearchHalls;

public static class SearchHallsEndpoint
{
    public const string Url = "/api/halls/search";

    public static string CreateSendableUrl(SearchHallsQuery query)
    {
        IEnumerable<KeyValuePair<string, string?>> queryParams =
        [
            new(nameof(query.BookingStart), query.BookingStart.ToString("O")),
            new(nameof(query.BookingDuration), query.BookingDuration.ToString()),
            new(nameof(query.Capacity), query.Capacity.ToString())
        ];
        return QueryHelpers.AddQueryString(Url, queryParams);
    }
    public static async Task<IResult> SearchHalls(
        [FromQuery] DateTime bookingStart,
        [FromQuery] TimeSpan bookingDuration,
        [FromQuery] int capacity,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        ErrorOr<IEnumerable<HallDto>> response = await mediator.Send(
            new SearchHallsQuery(bookingStart, bookingDuration, capacity),
            cancellationToken
        );
        return response.ToHttpResult();
    }

    extension(RouteHandlerBuilder thisBuilder)
    {
        public RouteHandlerBuilder AddSearchHallsProductionProblems()
        {
            return thisBuilder.ProducesValidationProblem(StatusCodes.Status422UnprocessableEntity);
        }
    }
    extension(WebApplication thisApp)
    {
        public void AddSearchHallsEndpoint()
        {
            thisApp.MapGet(Url, SearchHalls)
                .WithName(nameof(SearchHalls))
                .Produces<IEnumerable<HallDto>>(StatusCodes.Status200OK)
                .AddSearchHallsProductionProblems();
        }
    }
    extension(HttpClient thisHttpClient)
    {
        public async ValueTask<HttpResponseMessage> SendSearchHallsAsync(SearchHallsQuery query, CancellationToken cancellationToken)
        {
            using HttpRequestMessage request = new(HttpMethod.Get, CreateSendableUrl(query));
            return await thisHttpClient.SendAsync(request, cancellationToken);
        }
    }
}