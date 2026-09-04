using Bogus;
using System.Net;
using Microsoft.EntityFrameworkCore;
using HallRentingService.Data.Features.Halls;
using HallRentingService.Data.Features.HallServices;
using HallRentingService.WebAPI.Features.Halls.Dtos;
using HallRentingService.Data.Features.Halls_HallServices;
using HallRentingService.WebAPI.Features.HallServices.Dtos;
using HallRentingService.WebAPI.Features.Halls_HallServices;
using HallRentingService.WebAPI.Features.Hall_HallServices.Dtos;
using HallRentingService.WebAPI.Features.Halls.Handlers.UpdateHall;

namespace HallRentingService.IntegrationTests.Features.Halls.Handlers.UpdateHall;

[ClassDataSource<AppFixture>(Shared = SharedType.PerTestSession)]
public sealed class UpdateHallHandlerTests(AppFixture thisApp)
{
    private Faker Faker { get; } = new();

    [Test]
    public async Task Handler_ShouldUpdateHall_OnSuccess()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        HallDto[] halls = await thisApp.ExecuteDbContextAsync(async db =>
        {
            await new Faker<HallEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
            await new Faker<HallServiceEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
            await new Faker<Hall_HallService_JoinEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
            return await db.Halls.AsNoTracking().Include(h => h.HallServices).ProjectToDto().ToArrayAsync();
        });
        HallDto hallToUpdate = Faker.PickRandom(halls);
        List<HallServiceEntity> newHallServices = await thisApp.ExecuteDbContextAsync(async db =>
        {
            return await new Faker<HallServiceEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
        });
        List<HallService_JoinDto> newJes = Faker.PickRandom(newHallServices, Faker.Random.Number(1, newHallServices.Count - 1)).Select(hs =>
        {
            return new Faker<Hall_HallService_JoinEntity>().Valid().WithHallServiceId(hs.Id).Generate().ToHallServiceDto();
        }).ToList();
        List<HallService_JoinDto> oldJes = Faker.PickRandom(hallToUpdate.HallServices, Faker.Random.Number(1, hallToUpdate.HallServices.Count - 1)).ToList();
        HallDto hall = new Faker<HallEntity>().Valid().Generate().ToDto();
        hall.Id = hallToUpdate.Id;
        hall.HallServices = [..oldJes, ..newJes];

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendUpdateHallAsync(hall, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.NoContent);
        await thisApp.ExecuteDbContextAsync(async db =>
        {
            HallEntity? entity = await db.Halls.AsNoTracking().Include(e => e.HallServices).SingleOrDefaultAsync(
                e => e.Id == hall.Id,
                TestContext.Current!.Execution.CancellationToken
            );
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
        HallDto[] halls = await thisApp.ExecuteDbContextAsync(async db =>
        {
            await new Faker<HallEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
            await new Faker<HallServiceEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
            await new Faker<Hall_HallService_JoinEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
            return await db.Halls.AsNoTracking().Include(h => h.HallServices).ProjectToDto().ToArrayAsync();
        });
        HallDto hallToUpdate = Faker.PickRandom(halls);
        List<HallServiceEntity> newHallServices = await thisApp.ExecuteDbContextAsync(async db =>
        {
            return await new Faker<HallServiceEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
        });
        List<HallService_JoinDto> newJes = Faker.PickRandom(newHallServices, Faker.Random.Number(1, newHallServices.Count - 1)).Select(hs =>
        {
            return new Faker<Hall_HallService_JoinEntity>().Valid().WithHallServiceId(Guid.NewGuid()).Generate().ToHallServiceDto();
        }).ToList();
        List<HallService_JoinDto> oldJes = Faker.PickRandom(hallToUpdate.HallServices, Faker.Random.Number(1, hallToUpdate.HallServices.Count - 1)).ToList();
        HallDto hall = new Faker<HallEntity>().Valid().Generate().ToDto();
        hall.Id = hallToUpdate.Id;
        hall.HallServices = [..oldJes, ..newJes];

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendUpdateHallAsync(hall, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Handler_ShouldFail_WhenIdNotFound()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        HallDto hall = new Faker<HallEntity>().Valid().Generate().ToDto();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendUpdateHallAsync(hall, TestContext.Current!.Execution.CancellationToken);

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
        using HttpResponseMessage message = await thisApp.HttpClient.SendUpdateHallAsync(hall, TestContext.Current!.Execution.CancellationToken);

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
        using HttpResponseMessage message = await thisApp.HttpClient.SendUpdateHallAsync(hall, TestContext.Current!.Execution.CancellationToken);

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
        using HttpResponseMessage message = await thisApp.HttpClient.SendUpdateHallAsync(hall, TestContext.Current!.Execution.CancellationToken);

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
        using HttpResponseMessage message = await thisApp.HttpClient.SendUpdateHallAsync(hall, TestContext.Current!.Execution.CancellationToken);

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
        using HttpResponseMessage message = await thisApp.HttpClient.SendUpdateHallAsync(hall, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.UnprocessableEntity);
    }
}