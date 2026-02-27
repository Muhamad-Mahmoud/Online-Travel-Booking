using FluentValidation;
using MediatR;

namespace OnlineTravel.Application.Common.Behaviors;

/// <summary>
/// Pipeline Behavior for automatic validation of commands and queries.
/// Runs all registered validators for a request before it reaches the handler.
/// If validation fails, throws ValidationException with all error messages.
/// </summary>
public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
{
	private readonly IEnumerable<IValidator<TRequest>> _validators;

	public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
	{
		_validators = validators;
	}

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		if (!_validators.Any())
		{
			return await next();
		}

		var context = new ValidationContext<TRequest>(request);
		var validationResults = await Task.WhenAll(
			_validators.Select(v => v.ValidateAsync(context, cancellationToken))
		);

		var failures = validationResults
			.Where(r => !r.IsValid)
			.SelectMany(r => r.Errors)
			.ToList();

		if (failures.Any())
		{
			throw new ValidationException(failures);
		}

		return await next();
	}
}
