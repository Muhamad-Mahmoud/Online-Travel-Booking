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
		// Register Mapster
		var config = TypeAdapterConfig.GlobalSettings;
		config.Scan(typeof(DependencyInjection).Assembly);
		services.AddSingleton(config);
		services.AddScoped<IMapper, ServiceMapper>();

		// Register FluentValidation + Pipeline Behavior
		services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


		// Register Business Services
		services.AddScoped<IBookingStrategy, HotelBookingStrategy>();
		services.AddScoped<IBookingStrategy, TourBookingStrategy>();
		services.AddScoped<IBookingStrategy, FlightBookingStrategy>();
		services.AddScoped<IBookingStrategy, CarBookingStrategy>();


		return services;
	}
}

