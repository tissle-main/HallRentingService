using Bogus;
using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using HallRentingService.Data.Features.HallServices;
using HallRentingService.WebAPI.Features.HallService.Dtos;
using HallRentingService.WebAPI.Features.HallServices.Dtos;
using HallRentingService.WebAPI.Features.HallServices.Handlers.CreateHallService;

namespace HallRentingService.IntegrationTests.Features.HallServices.Handlers.CreateHallService;

[ClassDataSource<AppFixture>(Shared = SharedType.PerTestSession)]
public sealed class CreateHallServiceHandlerTests(AppFixture thisApp)
{
    private Faker Faker { get; } = new();

    [Test]
    public async Task Handler_ShouldCreateHallService_OnSuccess()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        HallServiceDto hallService = new Faker<HallServiceEntity>().Valid().Generate().ToDto();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendCreateHallServiceAsync(hallService, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.OK);
        Guid? id = await message.Content.ReadFromJsonAsync<Guid?>(TestContext.Current!.Execution.CancellationToken);
        await Assert.That(id).IsNotNull();
        await thisApp.ExecuteDbContextAsync(async db =>
        {
            hallService.Id = id.GetValueOrDefault();
            HallServiceEntity? entity = await db.HallServices.AsNoTracking().SingleOrDefaultAsync(TestContext.Current!.Execution.CancellationToken);
            await Assert.That(entity).IsNotNull();
            await Assert.That(hallService).IsEquivalentTo(entity.ToDto());
        });
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
        HallServiceDto hallService = Faker.PickRandom(hallServices).ToDto();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendCreateHallServiceAsync(hallService, TestContext.Current!.Execution.CancellationToken);

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
        using HttpResponseMessage message = await thisApp.HttpClient.SendCreateHallServiceAsync(hallService, TestContext.Current!.Execution.CancellationToken);

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
        using HttpResponseMessage message = await thisApp.HttpClient.SendCreateHallServiceAsync(hallService, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.UnprocessableEntity);
    }
}