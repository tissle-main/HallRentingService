using HallRentingService.WebAPI.Features.Hall_HallServices.Handlers;

namespace HallRentingService.WebAPI.Features.Hall_HallServices;

public sealed class Hall_HallServices_FeatureProvider : FeatureProvider
{
    #region Base
    public override void UseMiddleware(WebApplication app)
    {
        if(app.Environment.IsEnvironment(ProfileNames.Test))
        {
            app.AddUpdateHall_HallServicesEndpoint();
        }
    }
    #endregion
}