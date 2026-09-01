using ErrorOr;
using Mediator;
using HallRentingService.Data;
using Microsoft.EntityFrameworkCore;
using HallRentingService.WebAPI.Features.Halls.Dtos;

namespace HallRentingService.WebAPI.Features.Halls.Handlers.GetHalls;

public sealed class GetHallsHandler(AppDbContext thisDbContext) : IQueryHandler<GetHallsQuery, ErrorOr<IEnumerable<HallDto>>>
{
    #region Interfaces
    public async ValueTask<ErrorOr<IEnumerable<HallDto>>> Handle(GetHallsQuery query, CancellationToken cancellationToken)
    {
        if(query.Ids.Length == 0)
        {
            return await thisDbContext.Halls.Include(e => e.HallServices).Include(e => e.Bookings).ProjectToDto().ToArrayAsync(cancellationToken);
        }
        Guid[] ids = query.Ids.Distinct().ToArray();
        HallDto[] dtos = await thisDbContext.Halls.Include(e => e.HallServices).Include(e => e.Bookings).Where(
            e => ids.Contains(e.Id)    
        ).ProjectToDto().ToArrayAsync(cancellationToken);
        if(ids.Length > dtos.Length)
        {
            return Error.NotFound();
        }
        return dtos;
    }
    #endregion
}