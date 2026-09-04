using ErrorOr;
using Mediator;
using HallRentingService.Data;
using Microsoft.EntityFrameworkCore;
using HallRentingService.Data.Features.Halls;
using HallRentingService.Data.Features.Booking;
using HallRentingService.WebAPI.Features.Halls;
using HallRentingService.WebAPI.Features.HallServices;
using HallRentingService.WebAPI.Features.Booking.Services;
using HallRentingService.Data.Features.Halls_HallServices;

namespace HallRentingService.WebAPI.Features.Booking.Handlers.BookHall;

public sealed class BookHallHandler(
    AppDbContext thisDbContext,
    IBookingPriceModifier bookingPriceModifier
) : ICommandHandler<BookHallCommand, ErrorOr<BookHallResponse>>
{
    #region Interfaces
    public async ValueTask<ErrorOr<BookHallResponse>> Handle(BookHallCommand command, CancellationToken cancellationToken)
    {
        DateTime bookingEnd = command.BookingStart.Add(command.BookingDuration);
        Guid[] hallServiceIds = command.HallServices.Distinct().ToArray();
        HallEntity? hall = await thisDbContext.Halls.Include(e => e.HallServices).Include(e => e.Bookings).FirstOrDefaultAsync(
            e => e.Id == command.HallId,
            cancellationToken
        );
        if(hall is null)
        {
            return HallErrors.IdsNotFound([command.HallId]);
        }
        if(hall.Bookings.Any(booking => booking.BookingStart < bookingEnd && command.BookingStart < booking.BookingStart.Add(booking.BookingDuration)))
        {
            return BookingErrors.BookingOverlaps(command.BookingStart, command.BookingDuration);
        }
        Hall_HallService_JoinEntity[] hallServices = hall.HallServices.Where(je => hallServiceIds.Contains(je.HallServiceId)).ToArray();
        if(hallServices.Length != hallServiceIds.Length)
        {
            Guid[] missingIds = hallServiceIds.Except(hallServices.Select(je => je.HallServiceId)).ToArray();
            return HallServiceErrors.IdsNotFound(missingIds);
        }
        float totalPrice = bookingPriceModifier.ApplyModifiers(hall.PricePerHour, command.BookingStart, command.BookingDuration);
        totalPrice += hallServices.Sum(hallService => hallService.Price);
        BookingEntity bookingEntity = new()
        {
            BookingStart = command.BookingStart,
            BookingDuration = command.BookingDuration,
            TotalPrice = totalPrice,
            HallId = hall.Id,
            Hall = hall
        };
        await thisDbContext.Bookings.AddAsync(bookingEntity, cancellationToken);
        await thisDbContext.SaveChangesAsync(cancellationToken);
        return new BookHallResponse(bookingEntity.Id, bookingEntity.TotalPrice);
    }
    #endregion
}