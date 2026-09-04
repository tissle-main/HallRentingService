using Bogus;
using System.Net;
using System.Net.Http.Json;
using HallRentingService.Data.Features.HallServices;
using HallRentingService.WebAPI.Features.HallService.Dtos;
using HallRentingService.WebAPI.Features.HallServices.Dtos;
using HallRentingService.WebAPI.Features.HallServices.Handlers.GetHallServices;

namespace HallRentingService.IntegrationTests.Features.HallServices.Handlers.GetHallServices;

[ClassDataSource<AppFixture>(Shared = SharedType.PerTestSession)]
public sealed class GetHallServicesHandlerTests(AppFixture thisApp)
{
    private Faker Faker { get; } = new();

    [Test]
    public async Task Handler_ShouldReturnAllHallServices_WhenNoIdsProvided()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        List<HallServiceEntity> hallServices = await thisApp.ExecuteDbContextAsync(async db =>
        {
            return await new Faker<HallServiceEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
        });

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendGetHallServicesAsync([], TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.OK);
        HallServiceDto[]? result = await message.Content.ReadFromJsonAsync<HallServiceDto[]>(TestContext.Current!.Execution.CancellationToken);
        await Assert.That(result).IsNotNull();
        await Assert.That(result).IsEquivalentTo(hallServices.ToDtos());
    }

    [Test]
    public async Task Handler_ShouldReturnConcreteHallServices_WhenIdsProvided()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        List<HallServiceEntity> hallServices = await thisApp.ExecuteDbContextAsync(async db =>
        {
            return await new Faker<HallServiceEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
        });
        hallServices = Faker.PickRandom(hallServices, Faker.Random.Number(1, hallServices.Count - 1)).ToList();
        Guid[] ids = hallServices.Select(hall => hall.Id).ToArray();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendGetHallServicesAsync(ids, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.OK);
        HallServiceDto[]? result = await message.Content.ReadFromJsonAsync<HallServiceDto[]>(TestContext.Current!.Execution.CancellationToken);
        await Assert.That(result).IsNotNull();
        await Assert.That(result).IsEquivalentTo(hallServices.ToDtos());
    }

    [Test]
    public async Task Handler_ShouldFail_WhenEntitiesNotFound()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendGetHallServicesAsync([Guid.NewGuid()], TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
    }
}