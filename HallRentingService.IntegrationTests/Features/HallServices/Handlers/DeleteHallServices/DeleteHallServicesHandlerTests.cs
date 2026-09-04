using Bogus;
using System.Net;
using Microsoft.EntityFrameworkCore;
using HallRentingService.Data.Features.HallServices;
using HallRentingService.WebAPI.Features.HallServices.Dtos;
using HallRentingService.WebAPI.Features.HallServices.Handlers.DeleteHallServices;

namespace HallRentingService.IntegrationTests.Features.HallServices.Handlers.DeleteHallServices;

[ClassDataSource<AppFixture>(Shared = SharedType.PerTestSession)]
public sealed class DeleteHallServicesHandlerTests(AppFixture thisApp)
{
    private Faker Faker { get; } = new();

    [Test]
    public async Task Handler_ShouldDeleteAllHallServices_WhenNoIdsProvided()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        List<HallServiceEntity> hallServices = await thisApp.ExecuteDbContextAsync(async db =>
        {
            return await new Faker<HallServiceEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
        });

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendDeleteHallServicesAsync([], TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.NoContent);
        await thisApp.ExecuteDbContextAsync(async db =>
        {
            bool hallServicesExist = await db.HallServices.AnyAsync(TestContext.Current!.Execution.CancellationToken);
            await Assert.That(hallServicesExist).IsFalse();
        });
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
        Guid[] ids = Faker.PickRandom(hallServices, Faker.Random.Number(1, hallServices.Count - 1)).Select(hall => hall.Id).ToArray();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendDeleteHallServicesAsync(ids, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.NoContent);
        await thisApp.ExecuteDbContextAsync(async db =>
        {
            HallServiceEntity[] actualHallServices = await db.HallServices.AsNoTracking().ToArrayAsync(TestContext.Current!.Execution.CancellationToken);
            await Assert.That(actualHallServices).IsEquivalentTo(hallServices.Where(h => !ids.Contains(h.Id)));
        });
    }

    [Test]
    public async Task Handler_ShouldFail_WhenEntitiesNotFound()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        List<HallServiceEntity> hallServices = await thisApp.ExecuteDbContextAsync(async db =>
        {
            return await new Faker<HallServiceEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
        });
        Guid[] ids = Enumerable.Range(0, Faker.Random.Number(1, hallServices.Count - 1)).Select(id => Faker.Random.Guid()).ToArray();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendDeleteHallServicesAsync(ids, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
        await thisApp.ExecuteDbContextAsync(async db =>
        {
            HallServiceEntity[] actualHallServices = await db.HallServices.AsNoTracking().ToArrayAsync(TestContext.Current!.Execution.CancellationToken);
            await Assert.That(actualHallServices).IsEquivalentTo(hallServices);
        });
    }
}