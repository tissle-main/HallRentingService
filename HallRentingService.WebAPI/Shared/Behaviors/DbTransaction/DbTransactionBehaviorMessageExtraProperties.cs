namespace HallRentingService.WebAPI.Shared.Behaviors.DbTransaction;

public sealed class DbTransactionBehaviorMessageExtraProperties
{
    public bool BeginDbTransaction { get; set; } = true;
    public bool RollbackOnError { get; set; } = true;
}