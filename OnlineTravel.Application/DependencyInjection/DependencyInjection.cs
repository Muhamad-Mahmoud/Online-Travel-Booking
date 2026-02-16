using FluentValidation;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OnlineTravel.Application.Common.Behaviors;
using OnlineTravel.Application.Features.Bookings.Strategies;

namespace OnlineTravel.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register MediatR for CQRS pattern
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly));
        services.AddMapster();

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
        services.AddSingleton(TypeAdapterConfig.GlobalSettings);
        services.AddScoped<IMapper, ServiceMapper>();
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);


        // Register Business Services
        services.AddScoped<IBookingStrategy, HotelBookingStrategy>();
        services.AddScoped<IBookingStrategy, TourBookingStrategy>();
        services.AddScoped<IBookingStrategy, FlightBookingStrategy>();
        services.AddScoped<IBookingStrategy, CarBookingStrategy>();


        return services;
    }
}

