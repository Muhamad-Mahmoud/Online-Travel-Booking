namespace Domain.ErrorHandling;

public static class EntityError<T>
{
    public static Error NotFound(string? customMessage = null) =>
        new($"{typeof(T).Name}.NotFound",
            customMessage ?? $"{typeof(T).Name} not found",
            404);

    public static Error Duplicated(string? customMessage = null) =>
        new($"{typeof(T).Name}.Duplicated",
            customMessage ?? $"Another {typeof(T).Name} already exists",
            409);

    public static Error InvalidData(string? customMessage) =>
        new($"{typeof(T).Name}.InvalidData",
            customMessage ?? $"Invalid {typeof(T).Name} data",
            400);

    public static Error OperationFailed(string? customMessage = null) =>
        new($"{typeof(T).Name}.OperationFailed",
            customMessage ?? $"Operation on {typeof(T).Name} failed",
            500);
}

