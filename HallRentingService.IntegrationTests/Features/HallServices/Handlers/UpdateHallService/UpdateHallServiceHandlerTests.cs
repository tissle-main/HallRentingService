using Bogus;
using System.Net;
using Microsoft.EntityFrameworkCore;
using HallRentingService.Data.Features.HallServices;
using HallRentingService.WebAPI.Features.HallService.Dtos;
using HallRentingService.WebAPI.Features.HallServices.Dtos;
using HallRentingService.WebAPI.Features.HallServices.Handlers.UpdateHallService;

namespace HallRentingService.IntegrationTests.Features.HallServices.Handlers.UpdateHallService;

[ClassDataSource<AppFixture>(Shared = SharedType.PerTestSession)]
public sealed class UpdateHallServiceHandlerTests(AppFixture thisApp)
{
    private Faker Faker { get; } = new();

    [Test]
    public async Task Handler_ShouldUpdateHallService_OnSuccess()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        List<HallServiceEntity> hallServices = await thisApp.ExecuteDbContextAsync(async db =>
        {
            return await new Faker<HallServiceEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
        });
        Guid id = Faker.PickRandom(hallServices).Id;
        HallServiceDto hallService = new Faker<HallServiceEntity>().Valid().WithId(id).Generate().ToDto();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendUpdateHallServiceAsync(hallService, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.NoContent);
        await thisApp.ExecuteDbContextAsync(async db =>
        {
            HallServiceEntity? entity = await db.HallServices.AsNoTracking().SingleOrDefaultAsync(
                e => e.Id == id,
                TestContext.Current!.Execution.CancellationToken
            );
            await Assert.That(entity).IsNotNull();
            await Assert.That(hallService).IsEquivalentTo(entity.ToDto());
        });
    }

    [Test]
    public async Task Handler_ShouldFail_WhenIdNotFound()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        HallServiceDto hallService = new Faker<HallServiceEntity>().Valid().Generate().ToDto();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendUpdateHallServiceAsync(hallService, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Handler_ShouldFail_WhenNameAlreadyExists()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        List<HallServiceEntity> hallServices = await thisApp.ExecuteDbContextAsync(async db =>
        {
            return await new Faker<HallServiceEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
        });
        Guid id = Faker.PickRandom(hallServices).Id;
        string name = Faker.PickRandom(hallServices.Where(hs => hs.Id != id)).Name;
        HallServiceDto hallService = new Faker<HallServiceEntity>().Valid().WithId(id).WithName(name).Generate().ToDto();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendUpdateHallServiceAsync(hallService, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.Conflict);
    }

    [Test]
    public async Task Handler_ShouldFail_WhenNameIsEmpty()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        HallServiceDto hallService = new Faker<HallServiceEntity>().Valid().WithEmptyName().Generate().ToDto();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendUpdateHallServiceAsync(hallService, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.UnprocessableEntity);
    }

    [Test]
    public async Task Handler_ShouldFail_WhenNameIsTooLarge()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        HallServiceDto hallService = new Faker<HallServiceEntity>().Valid().WithTooLargeName().Generate().ToDto();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendUpdateHallServiceAsync(hallService, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.UnprocessableEntity);
    }
}