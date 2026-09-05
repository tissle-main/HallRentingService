using Bogus;
using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using HallRentingService.Data.Features.Halls;
using HallRentingService.Data.Features.Booking;
using HallRentingService.Data.Features.HallServices;
using HallRentingService.WebAPI.Features.Halls.Dtos;
using HallRentingService.WebAPI.Features.Booking.Dtos;
using HallRentingService.Data.Features.Halls_HallServices;
using HallRentingService.WebAPI.Features.HallServices.Dtos;
using HallRentingService.WebAPI.Features.Halls_HallServices;
using HallRentingService.WebAPI.Features.Booking.Handlers.SearchHalls;

namespace HallRentingService.IntegrationTests.Features.Booking.SearchHalls;

[ClassDataSource<AppFixture>(Shared = SharedType.PerTestSession)]
public sealed class SearchHallsHandlerTests(AppFixture thisApp)
{
    private Faker Faker { get; } = new();

    [Test]
    public async Task Handler_ShouldReturnHalls_WithCapacityGreaterOrEqualToProvided()
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
        int capacity = Faker.PickRandom(halls).Capacity;
        halls = halls.Where(h => h.Capacity >= capacity).ToArray();
        SearchHallsQuery query = new Faker<SearchHallsQuery>().Valid().WithBookingStart(DateTime.UtcNow.AddMonths(1)).WithCapacity(capacity).Generate();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendSearchHallsAsync(query, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.OK);
        HallDto[]? result = await message.Content.ReadFromJsonAsync<HallDto[]>(TestContext.Current!.Execution.CancellationToken);
        await Assert.That(result).IsNotNull();
        await Assert.That(result).IsEquivalentTo(halls);
    }

    [Test]
    public async Task Handler_ShouldReturnHalls_WithBookingPeriodsNotOverlappingWithProvided()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        SearchHallsQuery query = new Faker<SearchHallsQuery>().Valid().WithBookingStart(DateTime.UtcNow.AddMonths(1)).WithCapacity(1).Generate();
        HallDto[] halls = await thisApp.ExecuteDbContextAsync(async db =>
        {
            List<HallEntity> seededHalls = await new Faker<HallEntity>().Valid().SeedDatabaseAsync(
                db,
                TestContext.Current!.Execution.CancellationToken,
                min: 6,
                max: 6
            );
            await new Faker<HallServiceEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
            await new Faker<Hall_HallService_JoinEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
           
            Guid[] invalidIds = seededHalls.Select(h => h.Id).Take(3).ToArray();
            BookingEntity firstBooking = new Faker<BookingEntity>().Valid()
                .WithBookingStart(query.BookingStart)
                .WithBookingDuration(query.BookingDuration)
                .WithHallId(seededHalls[0].Id);
            BookingEntity secondBooking = new Faker<BookingEntity>().Valid()
                .WithBookingStart(query.BookingStart.AddHours(-query.BookingDuration.TotalHours + 1))
                .WithBookingDuration(query.BookingDuration)
                .WithHallId(seededHalls[1].Id);
            BookingEntity thirdBooking = new Faker<BookingEntity>().Valid()
                .WithBookingStart(query.BookingStart.AddHours(query.BookingDuration.TotalHours - 1))
                .WithBookingDuration(query.BookingDuration)
                .WithHallId(seededHalls[2].Id);
            BookingEntity fourBooking = new Faker<BookingEntity>().Valid()
                .WithBookingStart(query.BookingStart.AddHours(-query.BookingDuration.TotalHours))
                .WithBookingDuration(query.BookingDuration)
                .WithHallId(seededHalls[3].Id);
            BookingEntity fifthBooking = new Faker<BookingEntity>().Valid()
                .WithBookingStart(query.BookingStart.AddHours(query.BookingDuration.TotalHours))
                .WithBookingDuration(query.BookingDuration)
                .WithHallId(seededHalls[4].Id);
            await db.Bookings.AddRangeAsync(
                [firstBooking, secondBooking, thirdBooking, fourBooking, fifthBooking],
                TestContext.Current!.Execution.CancellationToken
            );
            await db.SaveChangesAsync(TestContext.Current!.Execution.CancellationToken);
            await new Faker<BookingEntity>().Valid().WithHallId(seededHalls[5].Id).SeedDatabaseForHallAsync(db, TestContext.Current!.Execution.CancellationToken);
            return await db.Halls.AsNoTracking().Include(h => h.HallServices).Include(h => h.Bookings).Where(
                h => !invalidIds.Contains(h.Id)
            ).ProjectToDto().ToArrayAsync(TestContext.Current!.Execution.CancellationToken);
        });

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendSearchHallsAsync(query, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.OK);
        HallDto[]? result = await message.Content.ReadFromJsonAsync<HallDto[]>(TestContext.Current!.Execution.CancellationToken);
        await Assert.That(result).IsNotNull();
        await Assert.That(result).IsEquivalentTo(halls);
    }

    [Test]
    public async Task Handler_ShouldFail_WhenExpiredBookingStartProvided()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        SearchHallsQuery query = new Faker<SearchHallsQuery>().Valid().WithExpiredBookingStart().Generate();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendSearchHallsAsync(query, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.UnprocessableEntity);
    }

    [Test]
    public async Task Handler_ShouldFail_WhenZeroBookingDurationProvided()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        SearchHallsQuery query = new Faker<SearchHallsQuery>().Valid().WithZeroBookingDuration().Generate();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendSearchHallsAsync(query, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.UnprocessableEntity);
    }

    [Test]
    public async Task Handler_ShouldFail_WhenNegativeBookingDurationProvided()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        SearchHallsQuery query = new Faker<SearchHallsQuery>().Valid().WithNegativeBookingDuration().Generate();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendSearchHallsAsync(query, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.UnprocessableEntity);
    }

    [Test]
    public async Task Handler_ShouldFail_WhenZeroCapacityProvided()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        SearchHallsQuery query = new Faker<SearchHallsQuery>().Valid().WithZeroCapacity().Generate();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendSearchHallsAsync(query, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.UnprocessableEntity);
    }

    [Test]
    public async Task Handler_ShouldFail_WhenNegativeCapacityProvided()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        SearchHallsQuery query = new Faker<SearchHallsQuery>().Valid().WithNegativeCapacity().Generate();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendSearchHallsAsync(query, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.UnprocessableEntity);
    }
}