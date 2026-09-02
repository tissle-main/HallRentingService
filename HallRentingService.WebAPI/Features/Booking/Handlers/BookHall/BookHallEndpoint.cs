using ErrorOr;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using HallRentingService.WebAPI.Shared.Extensions;

namespace HallRentingService.WebAPI.Features.Booking.Handlers.BookHall;

public static class BookHallEndpoint
{
    public const string Url = "/api/hall/book";

    public static async Task<IResult> BookHall(
        [FromBody] BookHallCommand command,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        ErrorOr<BookHallResponse> response = await mediator.Send(command, cancellationToken);
        return response.ToHttpResult();
    }

    extension(RouteHandlerBuilder thisBuilder)
    {
        public RouteHandlerBuilder AddBookHallProductionProblems()
        {
            thisBuilder.ProducesProblem(StatusCodes.Status404NotFound);
            thisBuilder.ProducesProblem(StatusCodes.Status409Conflict);
            return thisBuilder.ProducesValidationProblem(StatusCodes.Status422UnprocessableEntity);
        }
    }
    extension(WebApplication thisApp)
    {
        public void AddBookHallEndpoint()
        {
            thisApp.MapPost(Url, BookHall)
                .WithName(nameof(BookHall))
                .Produces<BookHallResponse>(StatusCodes.Status200OK)
                .AddBookHallProductionProblems();
        }
    }
    extension(HttpClient thisHttpClient)
    {
        public async ValueTask<HttpResponseMessage> SendBookHallAsync(BookHallCommand command, CancellationToken cancellationToken)
        {
            using HttpRequestMessage request = new(HttpMethod.Post, Url)
            {
                Content = JsonContent.Create(command)
            };
            return await thisHttpClient.SendAsync(request, cancellationToken);
        }
    }
}