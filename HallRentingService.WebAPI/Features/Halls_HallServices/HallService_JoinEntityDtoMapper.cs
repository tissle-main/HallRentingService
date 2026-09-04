using Riok.Mapperly.Abstractions;
using HallRentingService.WebAPI.Features.Halls_HallServices;
using HallRentingService.Data.Features.Halls_HallServices;

namespace HallRentingService.WebAPI.Features.Hall_HallServices.Dtos;

[Mapper]
public static partial class HallService_JoinEntityDtoMapper
{
    [MapperIgnoreSource(nameof(Hall_HallService_JoinEntity.Hall))]
    [MapperIgnoreSource(nameof(Hall_HallService_JoinEntity.HallId))]
    [MapperIgnoreSource(nameof(Hall_HallService_JoinEntity.HallService))]
    public static partial HallService_JoinDto ToHallServiceDto(this Hall_HallService_JoinEntity entity);
    public static partial IEnumerable<HallService_JoinDto> ToHallServiceDtos(this IEnumerable<Hall_HallService_JoinEntity> entities);
    public static partial IQueryable<HallService_JoinDto> ProjectToHallServiceDto(this IQueryable<Hall_HallService_JoinEntity> query);

    [MapperIgnoreSource(nameof(Hall_HallService_JoinEntity.Hall))]
    [MapperIgnoreSource(nameof(Hall_HallService_JoinEntity.HallId))]
    [MapperIgnoreSource(nameof(Hall_HallService_JoinEntity.HallService))]
    public static partial void MapToHallServiceDto(this Hall_HallService_JoinEntity source, HallService_JoinDto destination);
    public static partial void MapToHallServiceDto(this HallService_JoinDto source, HallService_JoinDto destination);

    [MapperIgnoreTarget(nameof(Hall_HallService_JoinEntity.Hall))]
    [MapperIgnoreTarget(nameof(Hall_HallService_JoinEntity.HallId))]
    [MapperIgnoreTarget(nameof(Hall_HallService_JoinEntity.HallService))]
    public static partial Hall_HallService_JoinEntity ToEntity(this HallService_JoinDto dto);
    public static partial IEnumerable<HallService_JoinDto> ToEntities(this IEnumerable<Hall_HallService_JoinEntity> dtos);

    [MapperIgnoreTarget(nameof(Hall_HallService_JoinEntity.Hall))]
    [MapperIgnoreTarget(nameof(Hall_HallService_JoinEntity.HallId))]
    [MapperIgnoreTarget(nameof(Hall_HallService_JoinEntity.HallService))]
    public static partial void MapToEntity(this HallService_JoinDto source, Hall_HallService_JoinEntity destination);
    public static partial void MapToEntity(this Hall_HallService_JoinEntity source, Hall_HallService_JoinEntity destination);
}