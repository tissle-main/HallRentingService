using Scalar.AspNetCore;

namespace HallRentingService.WebAPI.Features.Scalar;

public sealed class ScalarFeatureProvider : FeatureProvider
{
    #region Base
    public override void AddServices(WebApplicationBuilder builder)
    {
        if(builder.Environment.IsDevelopment())
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddOpenApi();
        }
    }
    public override void UseMiddleware(WebApplication app)
    {
        if(app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
            });
        }
    }
    #endregion
}