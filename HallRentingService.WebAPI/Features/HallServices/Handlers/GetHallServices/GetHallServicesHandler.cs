using ErrorOr;
using Mediator;
using HallRentingService.Data;
using Microsoft.EntityFrameworkCore;
using HallRentingService.WebAPI.Features.HallService.Dtos;
using HallRentingService.WebAPI.Features.HallServices.Dtos;

namespace HallRentingService.WebAPI.Features.HallServices.Handlers.GetHallServices;

public sealed class GetHallServicesHandler(AppDbContext thisDbContext) : IQueryHandler<GetHallServicesQuery, ErrorOr<IEnumerable<HallServiceDto>>>
{
    #region Interfaces
    public async ValueTask<ErrorOr<IEnumerable<HallServiceDto>>> Handle(GetHallServicesQuery query, CancellationToken cancellationToken)
    {
        if(query.Ids.Length == 0)
        {
            return await thisDbContext.HallServices.AsNoTracking().ProjectToDto().ToArrayAsync(cancellationToken);
        }
        Guid[] ids = query.Ids.Distinct().ToArray();
        HallServiceDto[] hallServices = await thisDbContext.HallServices.AsNoTracking()
            .Where(e => ids.Contains(e.Id))
            .ProjectToDto()
            .ToArrayAsync(cancellationToken);
        if(ids.Length > hallServices.Length)
        {
            Guid[] missingIds = ids.Except(hallServices.Select(h => h.Id)).ToArray();
            return HallServiceErrors.IdsNotFound(missingIds);
        }
        return hallServices;
    }
    #endregion
}