using HallRentingService.AppHost;
using Arshid.Aspire.ApiDocs.Extensions;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);
IResourceBuilder<SqlServerServerResource> sqlserver = builder.AddSqlServer(AppHostResources.SqlServer);
IResourceBuilder<SqlServerDatabaseResource> database = sqlserver.AddDatabase(AppHostResources.AppDatabase);
IResourceBuilder<ProjectResource> web = builder.AddProject<Projects.HallRentingService_WebAPI>(AppHostResources.WebAPI);

web.WithReference(database).WaitFor(database).WithScalar(true).WithOpenApi(true);
await builder.Build().RunAsync();