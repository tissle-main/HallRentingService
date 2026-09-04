using Riok.Mapperly.Abstractions;
using HallRentingService.WebAPI.Features.Halls_HallServices;
using HallRentingService.Data.Features.Halls_HallServices;

namespace HallRentingService.WebAPI.Features.Hall_HallServices.Dtos;

[Mapper]
public static partial class Hall_JoinEntityDtoMapper
{
    [MapperIgnoreSource(nameof(Hall_HallService_JoinEntity.Hall))]
    [MapperIgnoreSource(nameof(Hall_HallService_JoinEntity.HallService))]
    [MapperIgnoreSource(nameof(Hall_HallService_JoinEntity.HallServiceId))]
    public static partial Hall_JoinDto ToHallDto(this Hall_HallService_JoinEntity entity);
    public static partial IEnumerable<Hall_JoinDto> ToHallDtos(this IEnumerable<Hall_HallService_JoinEntity> entities);

    [MapperIgnoreSource(nameof(Hall_HallService_JoinEntity.Hall))]
    [MapperIgnoreSource(nameof(Hall_HallService_JoinEntity.HallService))]
    [MapperIgnoreSource(nameof(Hall_HallService_JoinEntity.HallServiceId))]
    public static partial void MapToHallDto(this Hall_HallService_JoinEntity source, Hall_JoinDto destination);
    public static partial void MapToHallDto(this Hall_JoinDto source, Hall_JoinDto destination);
    public static partial IQueryable<Hall_JoinDto> ProjectToHallDto(this IQueryable<Hall_HallService_JoinEntity> query);

    [MapperIgnoreTarget(nameof(Hall_HallService_JoinEntity.Hall))]
    [MapperIgnoreTarget(nameof(Hall_HallService_JoinEntity.HallService))]
    [MapperIgnoreTarget(nameof(Hall_HallService_JoinEntity.HallServiceId))]
    public static partial Hall_HallService_JoinEntity ToEntity(this Hall_JoinDto dto);
    public static partial IEnumerable<Hall_JoinDto> ToEntities(this IEnumerable<Hall_HallService_JoinEntity> dtos);

    [MapperIgnoreTarget(nameof(Hall_HallService_JoinEntity.Hall))]
    [MapperIgnoreTarget(nameof(Hall_HallService_JoinEntity.HallService))]
    [MapperIgnoreTarget(nameof(Hall_HallService_JoinEntity.HallServiceId))]
    public static partial void MapToEntity(this Hall_JoinDto source, Hall_HallService_JoinEntity destination);
    public static partial void MapToEntity(this Hall_HallService_JoinEntity source, Hall_HallService_JoinEntity destination);
}