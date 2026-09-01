using HallRentingService.Data;
using Microsoft.EntityFrameworkCore;
using HallRentingService.Data.Shared.JoinEntities;
using HallRentingService.Data.Shared.KeyedEntities;

namespace HallRentingService.WebAPI.Shared.JoinEntities;

public static class JoinEntitiesDbSeeder
{
    extension<TJoinEntity, TLeftEntity, TRightEntity>(AppDbContext thisDbContext)
        where TJoinEntity : class, IJoinEntity<TJoinEntity, TLeftEntity, TRightEntity>, new()
        where TLeftEntity : class, IKeyedEntity
        where TRightEntity : class, IKeyedEntity
    {
        public async ValueTask SeedJoinEntitiesAsync(CancellationToken cancellationToken, Action<TJoinEntity>? initJoinEntity = null)
        {
            TLeftEntity[] lefts = await thisDbContext.Set<TLeftEntity>().ToArrayAsync(cancellationToken);
            TRightEntity[] rights = await thisDbContext.Set<TRightEntity>().ToArrayAsync(cancellationToken);
            foreach(TLeftEntity left in lefts)
            {
                foreach(TRightEntity right in rights)
                {
                    TJoinEntity join = new()
                    {
                        LeftId = left.Id,
                        RightId = right.Id
                    };
                    initJoinEntity?.Invoke(join);
                    await thisDbContext.Set<TJoinEntity>().AddAsync(join, cancellationToken);
                }
            }
            await thisDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}