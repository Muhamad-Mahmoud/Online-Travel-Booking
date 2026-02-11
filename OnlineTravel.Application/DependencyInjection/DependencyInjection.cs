using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OnlineTravel.Application.Common.Behaviors;

namespace OnlineTravel.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register MediatR for CQRS pattern
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly));

        // Register AutoMapper profiles
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);

        // Register FluentValidation validators manually
        var assembly = typeof(DependencyInjection).Assembly;
        var validatorType = typeof(IValidator<>);

        var validators = assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == validatorType))
            .ToList();

        foreach (var validator in validators)
        {
            var validatorInterface = validator.GetInterfaces()
                .First(i => i.IsGenericType &&
                    i.GetGenericTypeDefinition() == validatorType);
            services.AddScoped(validatorInterface, validator);
        }

        // Register Pipeline Behaviors for automatic validation
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
