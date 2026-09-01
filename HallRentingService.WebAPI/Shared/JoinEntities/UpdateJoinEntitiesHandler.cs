using ErrorOr;
using Mediator;
using HallRentingService.Data;
using Microsoft.EntityFrameworkCore;
using HallRentingService.Data.Shared.JoinEntities;
using HallRentingService.Data.Shared.KeyedEntities;

namespace HallRentingService.WebAPI.Shared.JoinEntities;

public abstract class UpdateJoinEntitiesHandler<TMessage, TJoinEntity, TLeftEntity, TRightEntity>(
    AppDbContext thisDbContext,
    Func<IEnumerable<Guid>, Error> leftIdsNotFound,
    Func<IEnumerable<Guid>, Error> rightIdsNotFound
) where TMessage :  IUpdateJoinEntitiesMessage<TJoinEntity, TLeftEntity, TRightEntity>
  where TJoinEntity : class, IJoinEntity<TJoinEntity, TLeftEntity, TRightEntity>
  where TLeftEntity : class, IKeyedEntity
  where TRightEntity : class, IKeyedEntity
{
    #region Static
    private static IEnumerable<TJoinEntity> JoinEntitiesToUpdate(TMessage message)
    {
        foreach(TJoinEntity newJe in message.NewEntities)
        {
            TJoinEntity? oldJe = message.OldEntities.FirstOrDefault(
                oldJe => JoinEntityKeyedEqualityComparer<TJoinEntity, TLeftEntity, TRightEntity>.Instance.Equals(oldJe, newJe)
            );
            if(oldJe is null)
            {
                continue;
            }
            if(oldJe.Equals(newJe))
            {
                continue;
            }
            yield return newJe;
        }
    }
    #endregion

    #region Instance
    private DbSet<TJoinEntity> JoinEntities { get; } = thisDbContext.Set<TJoinEntity>();
    private DbSet<TLeftEntity> LeftEntities { get; } = thisDbContext.Set<TLeftEntity>();
    private DbSet<TRightEntity> RightEntities { get; } = thisDbContext.Set<TRightEntity>();
    #endregion

    #region Interfaces
    public async ValueTask<ErrorOr<Unit>> Handle(TMessage message, CancellationToken cancellationToken)
    {
        if(message.OldEntities.Count == 0 && message.NewEntities.Count == 0)
        {
            return Unit.Value;
        }
        if(message.NewEntities.Count > 0)
        {
            Guid[] ids = message.NewEntities.Select(e => e.LeftId).Distinct().ToArray();
            TLeftEntity[] left = await LeftEntities.Where(e => ids.Contains(e.Id)).ToArrayAsync(cancellationToken);
            if(ids.Length > left.Length)
            {
                IEnumerable<Guid> leftIds = left.Select(e => e.Id);
                return leftIdsNotFound(ids.Except(leftIds));
            }

            ids = message.NewEntities.Select(e => e.RightId).Distinct().ToArray();
            TRightEntity[] right = await RightEntities.Where(e => ids.Contains(e.Id)).ToArrayAsync(cancellationToken);
            if(ids.Length > right.Length)
            {
                IEnumerable<Guid> rightIds = right.Select(e => e.Id);
                return rightIdsNotFound(ids.Except(rightIds));
            }
        }
        JoinEntities.RemoveRange(
            message.OldEntities.Except(message.NewEntities, JoinEntityKeyedEqualityComparer<TJoinEntity, TLeftEntity, TRightEntity>.Instance)
        );
        await JoinEntities.AddRangeAsync(          
            message.NewEntities.Except(message.OldEntities, JoinEntityKeyedEqualityComparer<TJoinEntity, TLeftEntity, TRightEntity>.Instance),
            cancellationToken
        );
        JoinEntities.UpdateRange(JoinEntitiesToUpdate(message));
        await thisDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
    #endregion
}