using FluentValidation;
using HallRentingService.Data;
using System.Collections.Frozen;
using Microsoft.EntityFrameworkCore;
using HallRentingService.WebAPI.Features;
using HallRentingService.ServiceDefaults;
using HallRentingService.Data.Features.Halls;
using HallRentingService.Data.Features.HallServices;
using HallRentingService.Data.Features.Halls_HallServices;
using HallRentingService.WebAPI.Shared.Behaviors.Validation;
using HallRentingService.WebAPI.Shared.Behaviors.DbTransaction;

namespace HallRentingService.WebAPI;

public static class DIContainer
{
    private static FrozenSet<FeatureProvider> Features { get; set; } = [];

    extension(WebApplicationBuilder thisBuilder)
    {
        public void AddCore()
        {
            thisBuilder.Services.AddDbContext<AppDbContext>(options =>
            {
                string? connectionStr = thisBuilder.Configuration.GetConnectionString(AppHostResources.AppDatabase);
                options.UseSqlServer(connectionStr, builder =>
                {
                    builder.MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name);
                });
            });
            thisBuilder.Services.AddProblemDetails(options =>
            {
                options.CustomizeProblemDetails = static void (ProblemDetailsContext ctx) =>
                {
                    ctx.ProblemDetails.Extensions.Add("instance", $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}");
                };
            });
            thisBuilder.AddCQRS();
            thisBuilder.AddFeatures();
        }
        public void AddCQRS()
        {
            thisBuilder.Services.AddMediator(options =>
            {
                options.ServiceLifetime = ServiceLifetime.Scoped;
                options.PipelineBehaviors = [
                    typeof(ValidationBehavior<,>),
                    typeof(DbTransactionBehavior<,>)
                ];
            });
            thisBuilder.Services.AddValidatorsFromAssemblyContaining(typeof(DIContainer), ServiceLifetime.Singleton);
            ValidatorOptions.Global.LanguageManager.Enabled = false;
        }
        public void AddFeatures()
        {
            Features = typeof(DIContainer).Assembly.GetTypes().Where(type =>
            {
                return !type.IsAbstract && type.IsAssignableTo(typeof(FeatureProvider));
            }).Select(type =>
            {
                return (FeatureProvider)Activator.CreateInstance(type)!;
            }).ToFrozenSet();
            foreach(FeatureProvider provider in Features)
            {
                provider.AddServices(thisBuilder);
            }
        }
    }
    extension(WebApplication thisApp)
    {
        public void UseCore()
        {
            thisApp.MigrateDatabase();
            thisApp.SeedDatabase();
            thisApp.UseFeatures();
        }
        public void MigrateDatabase()
        {
            using IServiceScope scope = thisApp.Services.CreateScope();
            AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.Migrate();
        }
        public void SeedDatabase()
        {
            using IServiceScope scope = thisApp.Services.CreateScope();
            AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            HallServiceEntity[] hallServices = [
                new HallServiceEntity()
                {
                    Name = "Projector"
                },
                new HallServiceEntity()
                {
                    Name = "Wi-Fi"
                },
                new HallServiceEntity()
                {
                    Name = "Sound"
                },
            ];
            dbContext.HallServices.AddRange(hallServices);
            dbContext.SaveChanges();

            HallEntity[] halls = [
                new HallEntity()
                {
                    Name = "Hall A",
                    Capacity = 50,
                    PricePerHour = 2000,
                    HallServices = [
                        new Hall_HallService_JoinEntity()
                        {
                            HallId = Guid.Empty,
                            HallServiceId = hallServices[0].Id,
                            Price = 500
                        },
                        new Hall_HallService_JoinEntity()
                        {
                            HallId = Guid.Empty,
                            HallServiceId = hallServices[1].Id,
                            Price = 300
                        },
                        new Hall_HallService_JoinEntity()
                        {
                            HallId = Guid.Empty,
                            HallServiceId = hallServices[2].Id,
                            Price = 700
                        },
                    ]
                },
                new HallEntity()
                {
                    Name = "Hall B",
                    Capacity = 100,
                    PricePerHour = 3500,
                    HallServices = [
                        new Hall_HallService_JoinEntity()
                        {
                            HallId = Guid.Empty,
                            HallServiceId = hallServices[0].Id,
                            Price = 500
                        },
                        new Hall_HallService_JoinEntity()
                        {
                            HallId = Guid.Empty,
                            HallServiceId = hallServices[1].Id,
                            Price = 300
                        },
                        new Hall_HallService_JoinEntity()
                        {
                            HallId = Guid.Empty,
                            HallServiceId = hallServices[2].Id,
                            Price = 700
                        },
                    ]
                },
                new HallEntity()
                {
                    Name = "Hall C",
                    Capacity = 30,
                    PricePerHour = 1500,
                    HallServices = [
                        new Hall_HallService_JoinEntity()
                        {
                            HallId = Guid.Empty,
                            HallServiceId = hallServices[0].Id,
                            Price = 500
                        },
                        new Hall_HallService_JoinEntity()
                        {
                            HallId = Guid.Empty,
                            HallServiceId = hallServices[1].Id,
                            Price = 300
                        },
                        new Hall_HallService_JoinEntity()
                        {
                            HallId = Guid.Empty,
                            HallServiceId = hallServices[2].Id,
                            Price = 700
                        },
                    ]
                },
            ];
            dbContext.Halls.AddRange(halls);
            dbContext.SaveChanges();
        }
        public void UseFeatures()
        {
            foreach(FeatureProvider provider in Features)
            {
                provider.UseMiddleware(thisApp);
            }
        }
    }
}