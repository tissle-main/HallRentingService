using ErrorOr;
using HallRentingService.Data.Features.HallServices;

namespace HallRentingService.WebAPI.Features.HallServices;

public static class HallServiceErrors
{
    private static string Code
    {
        get => field ??= nameof(HallServiceErrors)[..^"Errors".Length];
    }

    public static Error IdsNotFound(Guid[] ids)
    {
        string description = $"Some hall services not found. Missing ids: [{string.Join(", ", ids)}].";
        return Error.NotFound($"{Code}.{nameof(IdsNotFound)}", description);
    }
    public static Error NameAlreadyExists(string name)
    {
        string description = $"Hall service with '{nameof(HallServiceEntity.Name)}' = '{name}' already exists.";
        return Error.Conflict($"{Code}.{nameof(NameAlreadyExists)}", description);
    }
}