using System.Diagnostics.CodeAnalysis;
using HallRentingService.Data.Entities.Halls;
using HallRentingService.Data.Shared.JoinEntities;
using HallRentingService.Data.Entities.HallServices;

namespace HallRentingService.Data.Entities.Halls_HallServices;

public sealed class Hall_HallService_JoinEntity : IJoinEntity<Hall_HallService_JoinEntity, HallEntity, HallServiceEntity>
{
    #region Instance
    //Value properties
    public Guid LeftId { get; set; } //Interfaces
    public Guid RightId { get; set; } //Interfaces
    public int Price { get; set; }

    //Navigation properties
    public HallEntity? Left { get; set; } //Interfaces
    public HallServiceEntity? Right { get; set; } //Interfaces
    #endregion

    #region Base
    public override bool Equals(object? obj)
    {
        return Equals(obj as Hall_HallService_JoinEntity);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(LeftId, RightId, Price);
    }
    #endregion

    #region Interfaces
    public bool Equals([NotNullWhen(true)] Hall_HallService_JoinEntity? other)
    {
        if(other is null)
        {
            return false;
        }
        return this.LeftId == other.LeftId && this.RightId == other.RightId && this.Price == other.Price;
    }
    #endregion
}