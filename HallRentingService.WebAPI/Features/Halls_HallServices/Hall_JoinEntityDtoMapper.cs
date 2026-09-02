using Riok.Mapperly.Abstractions;
using HallRentingService.Data.Entities.Halls_HallServices;
using HallRentingService.WebAPI.Features.Halls_HallServices;

namespace HallRentingService.WebAPI.Features.Hall_HallServices.Dtos;

[Mapper]
public static partial class Hall_JoinEntityDtoMapper
{
    [MapperIgnoreSource(nameof(Hall_HallService_JoinEntity.Hall))]
    [MapperIgnoreSource(nameof(Hall_HallService_JoinEntity.HallService))]
    [MapperIgnoreSource(nameof(Hall_HallService_JoinEntity.HallServiceId))]
    public static partial Hall_JoinDto ToDto(this Hall_HallService_JoinEntity entity);
    public static partial IEnumerable<Hall_JoinDto> ToDtos(this IEnumerable<Hall_HallService_JoinEntity> entities);

    [MapperIgnoreSource(nameof(Hall_HallService_JoinEntity.Hall))]
    [MapperIgnoreSource(nameof(Hall_HallService_JoinEntity.HallService))]
    [MapperIgnoreSource(nameof(Hall_HallService_JoinEntity.HallServiceId))]
    public static partial void MapToDto(this Hall_HallService_JoinEntity source, Hall_JoinDto destination);
    public static partial void MapToDto(this Hall_JoinDto source, Hall_JoinDto destination);

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
    public static partial IQueryable<Hall_JoinDto> ProjectToDto(this IQueryable<Hall_HallService_JoinEntity> query);
}