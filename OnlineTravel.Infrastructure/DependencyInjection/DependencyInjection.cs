using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineTravel.Application.Features.Auth.Email;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Application.Interfaces.Services;
using OnlineTravel.Application.Interfaces.Services.Auth;
using OnlineTravel.Domain.Entities.Users;
using OnlineTravel.Infrastructure.Persistence.Context;
using OnlineTravel.Infrastructure.Persistence.Repositories;
using OnlineTravel.Infrastructure.Persistence.UnitOfWork;
using OnlineTravel.Infrastructure.Persistence.Seed;
using OnlineTravel.Infrastructure.Security;
using OnlineTravel.Infrastructure.Security.Jwt;
using OnlineTravel.Infrastructure.Services.Payments;

namespace OnlineTravel.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		// Add DbContext with SQL Server
		AddDatabaseContext(services, configuration);

		services.Configure<JwtOptions>(
				 configuration.GetSection("Jwt"));

		// Add Identity
		services.AddIdentityCore<AppUser>(options =>
		{
			// Configure identity options if needed
			options.Password.RequireDigit = false;
			options.Password.RequiredLength = 6;
			options.Password.RequireNonAlphanumeric = false;
			options.Password.RequireUppercase = false;
			options.Password.RequireLowercase = false;

			options.SignIn.RequireConfirmedEmail = true;

		})
		.AddRoles<IdentityRole<Guid>>()
		//.AddSignInManager<SignInManager<User>>()
		.AddEntityFrameworkStores<OnlineTravelDbContext>()
		.AddDefaultTokenProviders();


		// Add JWT Authentication
		services.AddJwtAuthentication(configuration);

		//Register Auth Services
		services.AddScoped<IAuthService, AuthService>();
		services.AddScoped<IJwtService, JwtService>();

		//Email
		services.Configure<EmailSettings>(
			configuration.GetSection("EmailSettings"));

		services.AddScoped<IEmailService, EmailService>();


		//Add AutoMapper
		// Add UnitOfWork
		services.AddScoped<IUnitOfWork, UnitOfWork>();

		// Add Payments
		services.Configure<StripeOptions>(configuration.GetSection(StripeOptions.SectionName));
		services.AddScoped<IPaymentService, StripePaymentService>();

		services.AddScoped<IHotelRepository, HotelRepository>();
		services.AddScoped<IRoomRepository, RoomRepository>();

		services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();

		return services;
	}

	private static void AddDatabaseContext(
		IServiceCollection services,
		IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("DefaultConnection")
			?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

		services.AddDbContext<OnlineTravelDbContext>(options =>
			options.UseSqlServer(connectionString,
				b => b.MigrationsAssembly(typeof(DependencyInjection).Assembly.FullName)
					  .UseNetTopologySuite()));
	}
}
