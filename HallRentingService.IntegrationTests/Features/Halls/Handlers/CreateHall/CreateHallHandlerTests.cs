using Bogus;
using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using HallRentingService.Data.Features.Halls;
using HallRentingService.Data.Features.HallServices;
using HallRentingService.WebAPI.Features.Halls.Dtos;
using HallRentingService.Data.Features.Halls_HallServices;
using HallRentingService.WebAPI.Features.HallServices.Dtos;
using HallRentingService.WebAPI.Features.Halls_HallServices;
using HallRentingService.WebAPI.Features.Hall_HallServices.Dtos;
using HallRentingService.WebAPI.Features.Halls.Handlers.CreateHall;

namespace HallRentingService.IntegrationTests.Features.Halls.Handlers.CreateHall;

[ClassDataSource<AppFixture>(Shared = SharedType.PerTestSession)]
public sealed class CreateHallHandlerTests(AppFixture thisApp)
{
    private Faker Faker { get; } = new();

    [Test]
    public async Task Handler_ShouldCreateHall_OnSuccess()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        List<HallServiceEntity> hallServices = await thisApp.ExecuteDbContextAsync(async db =>
        {
            return await new Faker<HallServiceEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
        });
        List<HallService_JoinDto> jes = Faker.PickRandom(hallServices, Faker.Random.Number(1, hallServices.Count)).Select(hs =>
        {
            return new Faker<Hall_HallService_JoinEntity>().Valid().WithHallServiceId(hs.Id).Generate().ToHallServiceDto();
        }).ToList();
        HallDto hall = new Faker<HallEntity>().Valid().Generate().ToDto();
        hall.HallServices = jes;

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendCreateHallAsync(hall, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.OK);
        Guid? id = await message.Content.ReadFromJsonAsync<Guid?>(TestContext.Current!.Execution.CancellationToken);
        await Assert.That(id).IsNotNull();
        await thisApp.ExecuteDbContextAsync(async db =>
        {
            hall.Id = id.GetValueOrDefault();
            HallEntity? entity = await db.Halls.AsNoTracking().Include(e => e.HallServices).SingleOrDefaultAsync(TestContext.Current!.Execution.CancellationToken);
            await Assert.That(entity).IsNotNull();
            HallDto actualHall = entity.ToDto();
            await Assert.That(hall).IsEquivalentTo(actualHall).IgnoringMember(nameof(HallDto.HallServices));
            await Assert.That(hall.HallServices).IsEquivalentTo(actualHall.HallServices);
        });
    }

    [Test]
    public async Task Handler_ShouldFail_WhenHallServicesNotFound()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        List<HallServiceEntity> hallServices = await thisApp.ExecuteDbContextAsync(async db =>
        {
            return await new Faker<HallServiceEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
        });
        List<HallService_JoinDto> jes = Faker.PickRandom(hallServices, Faker.Random.Number(1, hallServices.Count)).Select(hs =>
        {
            return new Faker<Hall_HallService_JoinEntity>().Valid().WithHallServiceId(Guid.NewGuid()).Generate().ToHallServiceDto();
        }).ToList();
        HallDto hall = new Faker<HallEntity>().Valid().Generate().ToDto();
        hall.HallServices = jes;

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendCreateHallAsync(hall, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Handler_ShouldFail_WhenNameIsEmpty()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        HallDto hall = new Faker<HallEntity>().Valid().WithEmptyName().Generate().ToDto();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendCreateHallAsync(hall, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.UnprocessableEntity);
    }

    [Test]
    public async Task Handler_ShouldFail_WhenNameIsTooLarge()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        HallDto hall = new Faker<HallEntity>().Valid().WithTooLargeName().Generate().ToDto();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendCreateHallAsync(hall, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.UnprocessableEntity);
    }

    [Test]
    public async Task Handler_ShouldFail_WhenCapacityIsZero()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        HallDto hall = new Faker<HallEntity>().Valid().WithZeroCapacity().Generate().ToDto();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendCreateHallAsync(hall, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.UnprocessableEntity);
    }

    [Test]
    public async Task Handler_ShouldFail_WhenCapacityIsNegative()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        HallDto hall = new Faker<HallEntity>().Valid().WithNegativeCapacity().Generate().ToDto();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendCreateHallAsync(hall, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.UnprocessableEntity);
    }

    [Test]
    public async Task Handler_ShouldFail_WhenPricePerHourIsNegative()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        HallDto hall = new Faker<HallEntity>().Valid().WithNegativePricePerHour().Generate().ToDto();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendCreateHallAsync(hall, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.UnprocessableEntity);
    }
}