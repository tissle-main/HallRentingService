using Bogus;
using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using HallRentingService.Data.Features.Halls;
using HallRentingService.Data.Features.Booking;
using HallRentingService.WebAPI.Features.Halls.Dtos;
using HallRentingService.WebAPI.Features.Booking.Dtos;
using HallRentingService.WebAPI.Features.Booking.Handlers.GetBookings;

namespace HallRentingService.IntegrationTests.Features.Booking.GetBookings;

[ClassDataSource<AppFixture>(Shared = SharedType.PerTestSession)]
public sealed class GetBookingsHandlerTests(AppFixture thisApp)
{
    private Faker Faker { get; } = new();

    [Test]
    public async Task Handler_ShouldReturnAllBookings_WhenNoIdsProvided()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        BookingDto[] bookings = await thisApp.ExecuteDbContextAsync(async db =>
        {
            await new Faker<HallEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
            await new Faker<BookingEntity>().Valid().SeedDatabaseForAllHallsAsync(db, TestContext.Current!.Execution.CancellationToken);
            return await db.Bookings.AsNoTracking().ProjectToDto().ToArrayAsync(TestContext.Current!.Execution.CancellationToken);
        });

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendGetBookingsAsync([], TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.OK);
        BookingDto[]? result = await message.Content.ReadFromJsonAsync<BookingDto[]>(TestContext.Current!.Execution.CancellationToken);
        await Assert.That(result).IsNotNull();
        await Assert.That(result).IsEquivalentTo(bookings);
    }

    [Test]
    public async Task Handler_ShouldReturnConcreteBookings_WhenIdsProvided()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        BookingDto[] bookings = await thisApp.ExecuteDbContextAsync(async db =>
        {
            await new Faker<HallEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
            await new Faker<BookingEntity>().Valid().SeedDatabaseForAllHallsAsync(db, TestContext.Current!.Execution.CancellationToken);
            return await db.Bookings.AsNoTracking().ProjectToDto().ToArrayAsync(TestContext.Current!.Execution.CancellationToken);
        });
        bookings = Faker.PickRandom(bookings, Faker.Random.Number(1, bookings.Length - 1)).ToArray();
        Guid[] ids = bookings.Select(b => b.Id).ToArray();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendGetBookingsAsync(ids, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.OK);
        BookingDto[]? result = await message.Content.ReadFromJsonAsync<BookingDto[]>(TestContext.Current!.Execution.CancellationToken);
        await Assert.That(result).IsNotNull();
        await Assert.That(result).IsEquivalentTo(bookings);
    }

    [Test]
    public async Task Handler_ShouldFail_WhenIdsNotFound()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendGetBookingsAsync([Guid.NewGuid()], TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
    }
}