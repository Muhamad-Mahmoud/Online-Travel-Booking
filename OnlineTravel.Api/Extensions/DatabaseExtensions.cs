using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineTravel.Domain.Entities.Users;
using OnlineTravel.Infrastructure.Persistence.Context;
using OnlineTravel.Infrastructure.Persistence.Seed;

namespace OnlineTravel.Api.Extensions;

public static class DatabaseExtensions
{
	public static async Task ApplyDatabaseSetupAsync(this WebApplication app)
	{
		using var scope = app.Services.CreateScope();
		var services = scope.ServiceProvider;
		var logger = services.GetRequiredService<ILogger<Program>>();

		try
		{
			var context = services.GetRequiredService<OnlineTravelDbContext>();
			var userManager = services.GetRequiredService<UserManager<AppUser>>();
			var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

			logger.LogInformation("Applying migrations...");
			await context.Database.MigrateAsync();

			logger.LogInformation("Seeding database...");
			await ApplicationDbContextSeed.SeedAsync(context, userManager, roleManager);

			logger.LogInformation("Database setup completed successfully.");
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "An error occurred during database setup (migration or seeding).");
			throw; // Re-throw to ensure startup fails if DB is broken
		}
	}
}

