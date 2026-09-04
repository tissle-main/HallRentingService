using Bogus;
using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using HallRentingService.Data.Features.Halls;
using HallRentingService.Data.Features.Booking;
using HallRentingService.WebAPI.Features.Halls.Dtos;
using HallRentingService.Data.Features.HallServices;
using HallRentingService.WebAPI.Features.Booking.Dtos;
using HallRentingService.Data.Features.Halls_HallServices;
using HallRentingService.WebAPI.Features.HallServices.Dtos;
using HallRentingService.WebAPI.Features.Halls_HallServices;
using HallRentingService.WebAPI.Features.Halls.Handlers.GetHalls;

namespace HallRentingService.IntegrationTests.Features.Halls.Handlers.GetHalls;

[ClassDataSource<AppFixture>(Shared = SharedType.PerTestSession)]
public sealed class GetHallsHandlerTests(AppFixture thisApp)
{
    private Faker Faker { get; } = new();

    [Test]
    public async Task Handler_ShouldReturnAllHalls_WhenNoIdsProvided()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        HallDto[] halls = await thisApp.ExecuteDbContextAsync(async db =>
        {
            await new Faker<HallEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
            await new Faker<HallServiceEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
            await new Faker<BookingEntity>().Valid().SeedDatabaseForAllHallsAsync(db, TestContext.Current!.Execution.CancellationToken);
            await new Faker<Hall_HallService_JoinEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
            return await db.Halls.AsNoTracking().Include(h => h.HallServices).Include(h => h.Bookings).ProjectToDto().ToArrayAsync(
                TestContext.Current!.Execution.CancellationToken
            );
        });

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendGetHallsAsync([], TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.OK);
        HallDto[]? result = await message.Content.ReadFromJsonAsync<HallDto[]>(TestContext.Current!.Execution.CancellationToken);
        await Assert.That(result).IsNotNull();
        await Assert.That(result).IsEquivalentTo(halls);
    }

    [Test]
    public async Task Handler_ShouldReturnConcreteHalls_WhenIdsProvided()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        HallDto[] halls = await thisApp.ExecuteDbContextAsync(async db =>
        {
            await new Faker<HallEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
            await new Faker<HallServiceEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
            await new Faker<BookingEntity>().Valid().SeedDatabaseForAllHallsAsync(db, TestContext.Current!.Execution.CancellationToken);
            await new Faker<Hall_HallService_JoinEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
            return await db.Halls.AsNoTracking().Include(h => h.HallServices).Include(h => h.Bookings).ProjectToDto().ToArrayAsync();
        });
        Guid[] ids = Faker.PickRandom(halls, Faker.Random.Number(1, halls.Length - 1)).Select(h => h.Id).ToArray();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendGetHallsAsync(ids, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.OK);
        HallDto[]? result = await message.Content.ReadFromJsonAsync<HallDto[]>(TestContext.Current!.Execution.CancellationToken);
        await Assert.That(result).IsNotNull();
        await Assert.That(result).IsEquivalentTo(halls.Where(h => ids.Contains(h.Id)));
    }

    [Test]
    public async Task Handler_ShouldFail_WhenIdsNotFound()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendGetHallsAsync([Guid.NewGuid()], TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
    }
}