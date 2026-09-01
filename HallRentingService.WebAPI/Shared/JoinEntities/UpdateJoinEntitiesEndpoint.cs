namespace HallRentingService.WebAPI.Shared.JoinEntities;

public static class UpdateJoinEntitiesEndpoint
{
    extension(RouteHandlerBuilder thisBuilder)
    {
        public RouteHandlerBuilder AddUpdateJoinEntitiesProductionProblems()
        {
            return thisBuilder.ProducesProblem(StatusCodes.Status404NotFound);
        }
    }
}