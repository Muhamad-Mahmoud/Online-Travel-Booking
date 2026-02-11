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

        // Register Business Services
        services.AddScoped<Features.Bookings.Pricing.HotelBookingPricing>();
        services.AddScoped<Features.Bookings.Pricing.TourBookingPricing>();
        services.AddScoped<Features.Bookings.Pricing.FlightBookingPricing>();
        services.AddScoped<Features.Bookings.Pricing.CarBookingPricing>();

        return services;
    }
}

