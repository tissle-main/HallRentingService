using HallRentingService.Data.Shared.JoinEntities;
using HallRentingService.Data.Shared.KeyedEntities;

namespace HallRentingService.WebAPI.Shared.JoinEntities;

public sealed class JoinEntityKeyedEqualityComparer<TJoinEntity, TLeftEntity, TRightEntity> : IEqualityComparer<TJoinEntity>
    where TJoinEntity : class, IJoinEntity<TJoinEntity, TLeftEntity, TRightEntity>
    where TLeftEntity : class, IKeyedEntity
    where TRightEntity : class, IKeyedEntity
{
    #region Static
    public static JoinEntityKeyedEqualityComparer<TJoinEntity, TLeftEntity, TRightEntity> Instance
    {
        get => field ??= new JoinEntityKeyedEqualityComparer<TJoinEntity, TLeftEntity, TRightEntity>();
    }
    #endregion

    #region Interfaces
    public bool Equals(TJoinEntity? x, TJoinEntity? y)
    {
        if(object.ReferenceEquals(x, y))
        {
            return true;
        }
        if(x is null || y is null)
        {
            return false;
        }
        return x.LeftId == y.LeftId && x.RightId == y.RightId;
    }
    public int GetHashCode(TJoinEntity obj)
    {
        return HashCode.Combine(obj.LeftId, obj.RightId);
    }
    #endregion
}