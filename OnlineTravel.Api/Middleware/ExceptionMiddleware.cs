using System.Net;
using System.Text.Json;
using FluentValidation;
using OnlineTravel.Api.Errors;
using OnlineTravel.Domain.ErrorHandling;

namespace OnlineTravel.Api.Middleware;

public class ExceptionMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<ExceptionMiddleware> _logger;
	private readonly IHostEnvironment _env;

	public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
	{
		_env = env;
		_logger = logger;
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, ex.Message);
			await HandleExceptionAsync(context, ex);
		}
	}

	private async Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		context.Response.ContentType = "application/json";

		var statusCode = (int)HttpStatusCode.InternalServerError;
		var message = exception.Message;
		var details = _env.IsDevelopment() ? exception.StackTrace?.ToString() : null;
		object? errors = null;

		switch (exception)
		{
			case ValidationException validationException:
				statusCode = (int)HttpStatusCode.BadRequest;
				message = "Validation Failed";
				errors = validationException.Errors
					.GroupBy(e => e.PropertyName)
					.ToDictionary(
						g => g.Key,
						g => g.Select(e => e.ErrorMessage).ToArray()
					);
				break;
			case NotFoundException:
				statusCode = (int)HttpStatusCode.NotFound;
				break;
			case BadRequestException:
				statusCode = (int)HttpStatusCode.BadRequest;
				break;
			default:
				statusCode = (int)HttpStatusCode.InternalServerError;
				message = _env.IsDevelopment() ? exception.Message : "Internal Server Error";
				break;
		}

		context.Response.StatusCode = statusCode;

		var response = new ApiExceptionResponse(statusCode, message, details)
		{
			Errors = errors
		};

		var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
		var json = JsonSerializer.Serialize(response, options);

		await context.Response.WriteAsync(json);
	}
}

