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
using HallRentingService.WebAPI.Features.Booking.Services;
using HallRentingService.WebAPI.Features.HallServices.Dtos;
using HallRentingService.WebAPI.Features.Halls_HallServices;
using HallRentingService.WebAPI.Features.Booking.Handlers.BookHall;

namespace HallRentingService.IntegrationTests.Features.Booking.BookHall;

[ClassDataSource<AppFixture>(Shared = SharedType.PerTestSession)]
public sealed class BookHallHandlerTests(AppFixture thisApp)
{
    private Faker Faker { get; } = new();

    [Test]
    public async Task Handler_ShouldCountPrice_WhenBookingIsValid()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        HallDto[] halls = await thisApp.ExecuteDbContextAsync(async db =>
        {
            await new Faker<HallEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
            await new Faker<HallServiceEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
            await new Faker<Hall_HallService_JoinEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
            return await db.Halls.AsNoTracking().Include(h => h.HallServices).Include(h => h.Bookings).ProjectToDto().ToArrayAsync(
                TestContext.Current!.Execution.CancellationToken
            );
        });
        HallDto hall = Faker.PickRandom(halls);
        BookHallCommand command = new Faker<BookHallCommand>().Valid().WithHallId(hall.Id).WithHallServices(
            hall.HallServices.Select(jd => jd.HallServiceId).ToList()
        );
        float finalPrice = new BookingPriceModifier().ApplyModifiers(hall.PricePerHour, command.BookingStart, command.BookingDuration);
        finalPrice += hall.HallServices.Sum(jd => jd.Price);

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendBookHallAsync(command, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.OK);
        BookHallResponse? response = await message.Content.ReadFromJsonAsync<BookHallResponse>(TestContext.Current!.Execution.CancellationToken);
        await Assert.That(response).IsNotNull();
        await Assert.That(response!.TotalPrice).IsEqualTo(finalPrice);
        await thisApp.ExecuteDbContextAsync(async db =>
        {
            BookingEntity? entity = await db.Bookings.AsNoTracking().SingleOrDefaultAsync(TestContext.Current!.Execution.CancellationToken);
            await Assert.That(entity).IsNotNull();
            await Assert.That(entity).IsEquivalentTo(new BookingEntity()
            {
                Id = response.Id,
                BookingStart = command.BookingStart,
                BookingDuration = command.BookingDuration,
                TotalPrice = response.TotalPrice,
                HallId = hall.Id
            });
        });
    }

    [Test]
    [Arguments(0)]
    [Arguments(-1)]
    [Arguments(1)]
    public async Task Handler_ShouldFail_WhenBookingOverlapsExistingBooking(int hourOffset)
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        HallDto[] halls = await thisApp.ExecuteDbContextAsync(async db =>
        {
            await new Faker<HallEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
            await new Faker<BookingEntity>().Valid().SeedDatabaseForAllHallsAsync(db, TestContext.Current!.Execution.CancellationToken, min: 1, max: 1);
            return await db.Halls.AsNoTracking().ProjectToDto().ToArrayAsync(
                TestContext.Current!.Execution.CancellationToken
            );
        });
        HallDto hall = Faker.PickRandom(halls);
        BookingEntity booking = await thisApp.ExecuteDbContextAsync(async db =>
        {
            return await db.Bookings.AsNoTracking().SingleAsync(b => b.HallId == hall.Id, TestContext.Current!.Execution.CancellationToken);
        });
        BookHallCommand command = new Faker<BookHallCommand>().Valid()
            .WithHallId(hall.Id)
            .WithBookingStart(booking.BookingStart)
            .WithBookingDuration(booking.BookingDuration + TimeSpan.FromHours(hourOffset))
            .WithHallServices(
                hall.HallServices.Select(jd => jd.HallServiceId).ToList()
            );

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendBookHallAsync(command, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.Conflict);
    }

    [Test]
    public async Task Handler_ShouldFail_WhenHallNotFound()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        HallDto[] halls = await thisApp.ExecuteDbContextAsync(async db =>
        {
            await new Faker<HallEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
            await new Faker<HallServiceEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
            await new Faker<Hall_HallService_JoinEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
            return await db.Halls.AsNoTracking().Include(h => h.HallServices).Include(h => h.Bookings).ProjectToDto().ToArrayAsync(
                TestContext.Current!.Execution.CancellationToken
            );
        });
        HallDto hall = Faker.PickRandom(halls);
        BookHallCommand command = new Faker<BookHallCommand>().Valid().WithHallServices(
            hall.HallServices.Select(jd => jd.HallServiceId).ToList()
        );

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendBookHallAsync(command, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Handler_ShouldFail_WhenHallServiceNotFound()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        HallDto[] halls = await thisApp.ExecuteDbContextAsync(async db =>
        {
            await new Faker<HallEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
            await new Faker<HallServiceEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
            await new Faker<Hall_HallService_JoinEntity>().Valid().SeedDatabaseAsync(db, TestContext.Current!.Execution.CancellationToken);
            return await db.Halls.AsNoTracking().Include(h => h.HallServices).Include(h => h.Bookings).ProjectToDto().ToArrayAsync(
                TestContext.Current!.Execution.CancellationToken
            );
        });
        HallDto hall = Faker.PickRandom(halls);
        BookHallCommand command = new Faker<BookHallCommand>().Valid().WithHallId(hall.Id).WithHallServices([Guid.NewGuid()]);

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendBookHallAsync(command, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Handler_ShouldFail_WhenBookingStartExpired()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        BookHallCommand command = new Faker<BookHallCommand>().Valid().WithExpiredBookingStart();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendBookHallAsync(command, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.UnprocessableEntity);
    }

    [Test]
    public async Task Handler_ShouldFail_WhenBookingDurationIsZero()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        BookHallCommand command = new Faker<BookHallCommand>().Valid().WithZeroBookingDuration();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendBookHallAsync(command, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.UnprocessableEntity);
    }

    [Test]
    public async Task Handler_ShouldFail_WhenBookingDurationIsNegative()
    {
        //Arrange
        await thisApp.ResetDatabaseAsync(TestContext.Current!.Execution.CancellationToken);
        BookHallCommand command = new Faker<BookHallCommand>().Valid().WithNegativeBookingDuration();

        //Act
        using HttpResponseMessage message = await thisApp.HttpClient.SendBookHallAsync(command, TestContext.Current!.Execution.CancellationToken);

        //Assert
        await Assert.That(message.StatusCode).IsEqualTo(HttpStatusCode.UnprocessableEntity);
    }
}