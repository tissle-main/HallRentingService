var builder = DistributedApplication.CreateBuilder(args);
builder.AddProject<Projects.HallRentingService_WebAPI>("hallrentingservice-webapi");
builder.Build().Run();