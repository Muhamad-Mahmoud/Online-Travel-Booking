namespace OnlineTravelBookingTeamB.Errors;

public class ApiResponse
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public bool IsSuccess { get; set; }

    public ApiResponse(int statusCode, string? message = null, bool isSuccess = false)
    {
        StatusCode = statusCode;
        Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        IsSuccess = isSuccess;
    }

    private string? GetDefaultMessageForStatusCode(int statusCode)
    {
        return statusCode switch
        {
            200 => "Success",
            201 => "Resource created successfully",
            400 => "A bad request, you have made",
            401 => "Authorized, you are not",
            404 => "Resource found, it was not",
            500 => "Errors are the path to the dark side. Errors lead to anger. Anger leads to hate. Hate leads to career change",
            _ => null
        };
    }
}

public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; set; }

    public ApiResponse(int statusCode, T? data, string? message = null)
        : base(statusCode, message, true)
    {
        Data = data;
    }
}
