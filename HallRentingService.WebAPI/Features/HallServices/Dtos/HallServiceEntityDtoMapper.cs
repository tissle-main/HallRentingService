using Riok.Mapperly.Abstractions;
using HallRentingService.Data.Entities.HallServices;
using HallRentingService.WebAPI.Features.HallServices.Dtos;

namespace HallRentingService.WebAPI.Features.HallService.Dtos;

[Mapper]
public static partial class HallServiceEntityDtoMapper
{
    [MapperIgnoreSource(nameof(HallServiceEntity.Halls))]
    public static partial HallServiceDto ToDto(this HallServiceEntity entity);
    public static partial IEnumerable<HallServiceDto> ToDtos(this IEnumerable<HallServiceEntity> entities);

    [MapperIgnoreSource(nameof(HallServiceEntity.Halls))]
    public static partial void MapToDto(this HallServiceEntity source, HallServiceDto destination);
    public static partial void MapToDto(this HallServiceDto source, HallServiceDto destination);

    [MapperIgnoreTarget(nameof(HallServiceEntity.Halls))]
    public static partial HallServiceEntity ToEntity(this HallServiceDto dto);
    public static partial IEnumerable<HallServiceDto> ToEntities(this IEnumerable<HallServiceEntity> dtos);

    [MapperIgnoreTarget(nameof(HallServiceEntity.Halls))]
    public static partial void MapToEntity(this HallServiceDto source, HallServiceEntity destination);
    public static partial void MapToEntity(this HallServiceEntity source, HallServiceEntity destination);
    public static partial IQueryable<HallServiceDto> ProjectToDto(this IQueryable<HallServiceEntity> query);
}