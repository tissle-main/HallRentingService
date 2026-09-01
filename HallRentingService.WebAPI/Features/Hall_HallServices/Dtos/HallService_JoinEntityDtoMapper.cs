using Riok.Mapperly.Abstractions;
using HallRentingService.Data.Entities.Halls_HallServices;

namespace HallRentingService.WebAPI.Features.Hall_HallServices.Dtos;

[Mapper]
public static partial class HallService_JoinEntityDtoMapper
{
    [MapperIgnoreSource(nameof(Hall_HallService_JoinEntity.Left))]
    [MapperIgnoreSource(nameof(Hall_HallService_JoinEntity.Right))]
    [MapperIgnoreSource(nameof(Hall_HallService_JoinEntity.LeftId))]
    public static partial HallService_JoinDto ToDto(this Hall_HallService_JoinEntity entity);
    public static partial IEnumerable<HallService_JoinDto> ToDtos(this IEnumerable<Hall_HallService_JoinEntity> entities);

    [MapperIgnoreSource(nameof(Hall_HallService_JoinEntity.Left))]
    [MapperIgnoreSource(nameof(Hall_HallService_JoinEntity.Right))]
    [MapperIgnoreSource(nameof(Hall_HallService_JoinEntity.LeftId))]
    public static partial void MapToDto(this Hall_HallService_JoinEntity source, HallService_JoinDto destination);
    public static partial void MapToDto(this HallService_JoinDto source, HallService_JoinDto destination);

    [MapperIgnoreTarget(nameof(Hall_HallService_JoinEntity.Left))]
    [MapperIgnoreTarget(nameof(Hall_HallService_JoinEntity.Right))]
    [MapperIgnoreTarget(nameof(Hall_HallService_JoinEntity.LeftId))]
    public static partial Hall_HallService_JoinEntity ToEntity(this HallService_JoinDto dto);
    public static partial IEnumerable<HallService_JoinDto> ToEntities(this IEnumerable<Hall_HallService_JoinEntity> dtos);

    [MapperIgnoreTarget(nameof(Hall_HallService_JoinEntity.Left))]
    [MapperIgnoreTarget(nameof(Hall_HallService_JoinEntity.Right))]
    [MapperIgnoreTarget(nameof(Hall_HallService_JoinEntity.LeftId))]
    public static partial void MapToEntity(this HallService_JoinDto source, Hall_HallService_JoinEntity destination);
    public static partial void MapToEntity(this Hall_HallService_JoinEntity source, Hall_HallService_JoinEntity destination);
    public static partial IQueryable<HallService_JoinDto> ProjectToDto(this IQueryable<Hall_HallService_JoinEntity> query);
}