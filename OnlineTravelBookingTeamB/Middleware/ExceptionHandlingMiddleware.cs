using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace OnlineTravelBookingTeamB.Middleware
{
    public class ExceptionHandlingMiddleware
    {

        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = StatusCodes.Status500InternalServerError; // Default 500
            object errors = null;

            if (exception is ValidationException validationException)
            {
                code = StatusCodes.Status400BadRequest; // Validation error is 400
                errors = validationException.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = code;

            var result = JsonSerializer.Serialize(new
            {
                status = code,
                message = "Validation Failed",
                errors
            });

            return context.Response.WriteAsync(result);
        }
    }
}
