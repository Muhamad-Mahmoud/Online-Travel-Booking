using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Api.Errors;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Api.Extensions;

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
}



