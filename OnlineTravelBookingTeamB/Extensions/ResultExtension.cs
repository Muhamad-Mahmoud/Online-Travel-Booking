using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Domain.ErrorHandling;
using OnlineTravelBookingTeamB.Errors;
using AppResult = OnlineTravel.Application.Common;

namespace OnlineTravelBookingTeamB.Extensions;

public static class ResultExtension
{
    public static ObjectResult ToProblem(this Result result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("Cannot convert success result to a problem");

        var problem = Results.Problem(statusCode: result.Error.StatusCode);
        var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;

        problemDetails!.Extensions = new Dictionary<string, object?>
        {
            {
                "errors", new[]
                {
                    result.Error.Code,
                    result.Error.Description
                }
            }
        };

        return new ObjectResult(problemDetails);
    }

    public static ActionResult ToResponse<T>(this Result<T> result, int statusCode = 200)
    {
        return result.IsSuccess
            ? new ObjectResult(new ApiResponse<T>(statusCode, result.Value)) { StatusCode = statusCode }
            : result.ToProblem();
    }

    public static ActionResult ToResponse(this Result result, int statusCode = 200)
    {
        return result.IsSuccess
            ? new ObjectResult(new ApiResponse(statusCode, isSuccess: true)) { StatusCode = statusCode }
            : result.ToProblem();
    }

    public static ActionResult ToResponse<T>(this AppResult.Result<T> result, int statusCode = 200)
    {
        if (result.IsSuccess)
        {
            return new ObjectResult(new ApiResponse<T>(statusCode, result.Value)) { StatusCode = statusCode };
        }

        var problemDetails = new ProblemDetails
        {
            Status = 400,
            Title = "Bad Request",
            Detail = result.Error,
            Extensions = { ["errors"] = result.ValidationErrors ?? [] }
        };

        return new ObjectResult(problemDetails) { StatusCode = 400 };
    }
}

