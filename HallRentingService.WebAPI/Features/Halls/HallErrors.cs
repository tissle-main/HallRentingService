using ErrorOr;

namespace HallRentingService.WebAPI.Features.Halls;

public static class HallErrors
{
    private static string Code
    {
        get => field ??= nameof(HallErrors)[..^"Errors".Length];
    }

    public static Error IdsNotFound(Guid[] ids)
    {
        string description = $"Some halls not found. Missing ids: [{string.Join(", ", ids)}].";
        return Error.NotFound($"{Code}.{nameof(IdsNotFound)}", description);
    }
}