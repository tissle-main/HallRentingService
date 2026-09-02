using Riok.Mapperly.Abstractions;
using HallRentingService.Data.Entities.Booking;

namespace HallRentingService.WebAPI.Features.Booking.Dtos;

[Mapper]
public static partial class BookingEntityDtoMapper
{
    [MapperIgnoreSource(nameof(BookingEntity.Hall))]
    public static partial BookingDto ToDto(this BookingEntity entity);
    public static partial IEnumerable<BookingDto> ToDtos(this IEnumerable<BookingEntity> entities);

    [MapperIgnoreSource(nameof(BookingEntity.Hall))]
    public static partial void MapToDto(this BookingEntity source, BookingDto destination);
    public static partial void MapToDto(this BookingDto source, BookingDto destination);
    public static partial IQueryable<BookingDto> ProjectToDto(this IQueryable<BookingEntity> query);
}