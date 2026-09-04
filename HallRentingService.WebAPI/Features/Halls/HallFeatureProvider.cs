using HallRentingService.WebAPI.Features.Halls.Handlers.GetHalls;
using HallRentingService.WebAPI.Features.Halls.Handlers.CreateHall;
using HallRentingService.WebAPI.Features.Halls.Handlers.UpdateHall;
using HallRentingService.WebAPI.Features.Halls.Handlers.DeleteHalls;

namespace HallRentingService.WebAPI.Features.Halls;

public sealed class HallFeatureProvider : FeatureProvider
{
    #region Base
    public override void UseMiddleware(WebApplication app)
    {
        app.AddGetHallsEndpoint();
        app.AddCreateHallEndpoint();
        app.AddUpdateHallEndpoint();
        app.AddDeleteHallsEndpoint();
    }
    #endregion
}