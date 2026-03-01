namespace OnlineTravel.Application.Common;

public class Result<T>
{
	public bool IsSuccess { get; private set; }
	public T? Value { get; private set; }
	public string? Error { get; private set; }

	public IReadOnlyList<string>? ValidationErrors { get; private set; }

	public static Result<T> Success(T value) => new Result<T> { IsSuccess = true, Value = value };
	public static Result<T> Failure(string error) => new Result<T> { IsSuccess = false, Error = error };
	public static Result<T> ValidationFailure(IReadOnlyList<string> errors) => new Result<T> { IsSuccess = false, Error = "Validation failed.", ValidationErrors = errors };
}
