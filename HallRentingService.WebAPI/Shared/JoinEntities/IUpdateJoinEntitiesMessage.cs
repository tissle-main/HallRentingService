using HallRentingService.Data.Shared.JoinEntities;
using HallRentingService.Data.Shared.KeyedEntities;
using HallRentingService.WebAPI.Shared.Behaviors.DbTransaction;

namespace HallRentingService.WebAPI.Shared.JoinEntities;

public interface IUpdateJoinEntitiesMessage<TJoinEntity, TLeftEntity, TRightEntity> : IDbTransactionBehaviorMessage
    where TJoinEntity : class, IJoinEntity<TJoinEntity, TLeftEntity, TRightEntity>
    where TLeftEntity : class, IKeyedEntity
    where TRightEntity : class, IKeyedEntity
{
    public IReadOnlyCollection<TJoinEntity> OldEntities { get; }
    public IReadOnlyCollection<TJoinEntity> NewEntities { get; }
}