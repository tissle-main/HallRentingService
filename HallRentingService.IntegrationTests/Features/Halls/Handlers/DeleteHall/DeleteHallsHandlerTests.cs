using Bogus;
using System.Net;
using Microsoft.EntityFrameworkCore;
using HallRentingService.Data.Features.Halls;
using HallRentingService.Data.Features.Booking;
using HallRentingService.Data.Features.HallServices;
using HallRentingService.WebAPI.Features.Halls.Dtos;
using HallRentingService.WebAPI.Features.Booking.Dtos;
using HallRentingService.Data.Features.Halls_HallServices;
using HallRentingService.WebAPI.Features.HallServices.Dtos;
using HallRentingService.WebAPI.Features.Halls_HallServices;
using HallRentingService.WebAPI.Features.Halls.Handlers.DeleteHalls;

namespace HallRentingService.IntegrationTests.Features.Halls.Handlers.DeleteHall;

[ClassDataSource<AppFixture>(Shared = SharedType.PerTestSession)]
public sealed class DeleteHallsHandlerTests(AppFixture thisApp)
{
    private Faker Faker { get; } = new();

    [Test]
    public async Task Handler_ShouldDeleteAllHalls_WhenNoIdsProvided()
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

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendDeleteHallsAsync([], TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.NoContent);
        await thisApp.ExecuteDbContextAsync(async db =>
        {
            bool anyBookings = await db.Bookings.AnyAsync(TestContext.Current!.Execution.CancellationToken);
            bool anyJes = await db.Hall_HallServices.AnyAsync(TestContext.Current!.Execution.CancellationToken);
            bool anyHalls = await db.Halls.AnyAsync(TestContext.Current!.Execution.CancellationToken);
            await Assert.That(anyBookings).IsFalse();
            await Assert.That(anyJes).IsFalse();
            await Assert.That(anyHalls).IsFalse();
        });
    }

    [Test]
    public async Task Handler_ShouldDeleteConcreteHalls_WhenIdsProvided()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        List<HallEntity> halls = await thisApp.ExecuteDbContextAsync(async db =>
        {
            await new Faker<HallServiceEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
            return await new Faker<HallEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
        });
        List<BookingEntity> bookings = await thisApp.ExecuteDbContextAsync(async db =>
        {
            await new Faker<BookingEntity>().Valid().SeedDatabaseForAllHallsAsync(db, TestContext.Current!.Execution.CancellationToken);
            return await db.Bookings.AsNoTracking().ToListAsync(TestContext.Current!.Execution.CancellationToken);
        });
        List<Hall_HallService_JoinEntity> jes = await thisApp.ExecuteDbContextAsync(async db =>
        {
            return await new Faker<Hall_HallService_JoinEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
        });
        Guid[] ids = Faker.PickRandom(halls, Faker.Random.Number(1, halls.Count)).Select(dto => dto.Id).ToArray();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendDeleteHallsAsync(ids, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.NoContent);
        await thisApp.ExecuteDbContextAsync(async db =>
        {
            BookingEntity[] actualBookings = await db.Bookings.AsNoTracking().ToArrayAsync(
                TestContext.Current!.Execution.CancellationToken
            );
            Hall_HallService_JoinEntity[] actualJes = await db.Hall_HallServices.AsNoTracking().ToArrayAsync(
                TestContext.Current!.Execution.CancellationToken
            );
            HallEntity[] actualHalls = await db.Halls.AsNoTracking().ToArrayAsync(
                TestContext.Current!.Execution.CancellationToken
            );
            await Assert.That(actualBookings).IsEquivalentTo(
                bookings.Where(e => !ids.Contains(e.HallId))
            );
            await Assert.That(actualJes).IsEquivalentTo(
                jes.Where(e => !ids.Contains(e.HallId))
            );
            await Assert.That(actualHalls).IsEquivalentTo(
                halls.Where(e => !ids.Contains(e.Id))
            );
        });
    }

    [Test]
    public async Task Handler_ShouldFail_WhenIdsNotFound()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        List<HallEntity> halls = await thisApp.ExecuteDbContextAsync(async db =>
        {
            return await new Faker<HallEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
        });

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendDeleteHallsAsync([Guid.NewGuid()], TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
        await thisApp.ExecuteDbContextAsync(async db =>
        {
            HallEntity[] actualHalls = await db.Halls.AsNoTracking().ToArrayAsync(TestContext.Current!.Execution.CancellationToken);
            await Assert.That(actualHalls).IsEquivalentTo(halls);
        });
    }
}