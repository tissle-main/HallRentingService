using HallRentingService.WebAPI.Features.HallServices.Handlers.CreateHallService;
using HallRentingService.WebAPI.Features.HallServices.Handlers.DeleteHallServices;
using HallRentingService.WebAPI.Features.HallServices.Handlers.GetHallServices;
using HallRentingService.WebAPI.Features.HallServices.Handlers.UpdateHallService;

namespace HallRentingService.WebAPI.Features.HallServices;

public sealed class HallServiceFeatureProvider : FeatureProvider
{
    #region Base
    public override void UseMiddleware(WebApplication app)
    {
        app.AddGetHallServicesEndpoint();
        app.AddCreateHallServiceEndpoint();
        app.AddUpdateHallServiceEndpoint();
        app.AddDeleteHallServicesEndpoint();
    }
    #endregion
}