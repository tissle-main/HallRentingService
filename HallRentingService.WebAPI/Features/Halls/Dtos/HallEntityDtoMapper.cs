using Riok.Mapperly.Abstractions;
using HallRentingService.Data.Entities.Halls;
using HallRentingService.Data.Entities.Booking;
using HallRentingService.Data.Entities.Halls_HallServices;
using HallRentingService.WebAPI.Features.Hall_HallServices.Dtos;

namespace HallRentingService.WebAPI.Features.Halls.Dtos;

[Mapper]
[UseStaticMapper(typeof(HallService_JoinEntityDtoMapper))]
public static partial class HallEntityDtoMapper
{
    [UserMapping]
    private static List<Hall_HallService_JoinEntity> CreateHall_HallServices_List(HallDto dto)
    {
        return dto.HallServices.Select(jd =>
        {
            Hall_HallService_JoinEntity je = jd.ToEntity();
            je.HallId = dto.Id;
            return je;
        }).ToList();
    }

    [UserMapping]
    private static Guid GetBookingId(BookingEntity booking)
    {
        return booking.Id;
    }

    public static partial HallDto ToDto(this HallEntity entity);
    public static partial IEnumerable<HallDto> ToDtos(this IEnumerable<HallEntity> entities);
    public static partial void MapToDto(this HallEntity source, HallDto destination);
    public static partial void MapToDto(this HallDto source, HallDto destination);

    [MapperIgnoreTarget(nameof(HallEntity.Bookings))]
    [MapPropertyFromSource(nameof(HallEntity.HallServices))]
    public static partial HallEntity ToEntity(this HallDto dto);
    public static partial IEnumerable<HallDto> ToEntities(this IEnumerable<HallEntity> dtos);

    [MapperIgnoreTarget(nameof(HallEntity.Bookings))]
    [MapPropertyFromSource(nameof(HallEntity.HallServices))]
    public static partial void MapToEntity(this HallDto source, HallEntity destination);
    public static partial void MapToEntity(this HallEntity source, HallEntity destination);
    public static partial IQueryable<HallDto> ProjectToDto(this IQueryable<HallEntity> query);
}