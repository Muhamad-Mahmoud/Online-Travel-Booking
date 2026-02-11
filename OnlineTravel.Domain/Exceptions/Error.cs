namespace OnlineTravel.Domain.ErrorHandling;

public record Error(string Code, string Description, int? StatusCode)
{
    public static readonly Error None = new(string.Empty, string.Empty, null);

    public static Error Unauthorized(string description)
        => new("Unauthorized", description, 401);

    public static Error Forbidden(string description)
        => new("Forbidden", description, 403);

    public static Error Validation(string description)
        => new("Validation", description, 400);

    public static Error NotFound(string description)
        => new("NotFound", description, 404);

    public static Error Conflict(string description)
        => new("Conflict", description, 409);

    public static Error InternalServer(string description)
        => new("InternalServer", description, 500);
}

