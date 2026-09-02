using ErrorOr;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using HallRentingService.WebAPI.Shared.Extensions;
using HallRentingService.WebAPI.Features.Booking.Dtos;

namespace HallRentingService.WebAPI.Features.Booking.Handlers.GetBookings;

public static class GetBookingsEndpoint
{
    public const string Url = "/api/bookings";

    public static string CreateSendableUrl(Guid[] ids)
    {
        IEnumerable<KeyValuePair<string, string?>> queryParams = ids.Select(id =>
        {
            return new KeyValuePair<string, string?>(nameof(ids), id.ToString());
        });
        return QueryHelpers.AddQueryString(Url, queryParams);
    }
    public static async Task<IResult> GetBookings(
        [FromQuery] Guid[] ids,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        ErrorOr<IEnumerable<BookingDto>> response = await mediator.Send(new GetBookingsQuery(ids), cancellationToken);
        return response.ToHttpResult();
    }

    extension(RouteHandlerBuilder thisBuilder)
    {
        public RouteHandlerBuilder AddGetBookingsProductionProblems()
        {
            return thisBuilder.ProducesProblem(StatusCodes.Status404NotFound);
        }
    }
    extension(WebApplication thisApp)
    {
        public void AddGetBookingsEndpoint()
        {
            thisApp.MapGet(Url, GetBookings)
                .WithName(nameof(GetBookings))
                .Produces<IEnumerable<BookingDto>>(StatusCodes.Status200OK)
                .AddGetBookingsProductionProblems();
        }
    }
    extension(HttpClient thisHttpClient)
    {
        public async ValueTask<HttpResponseMessage> SendGetBookingsAsync(Guid[] ids, CancellationToken cancellationToken)
        {
            using HttpRequestMessage request = new(HttpMethod.Get, CreateSendableUrl(ids));
            return await thisHttpClient.SendAsync(request, cancellationToken);
        }
    }
}