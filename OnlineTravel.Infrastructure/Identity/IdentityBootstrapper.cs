using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace OnlineTravel.Infrastructure.Identity;

public static class IdentityBootstrapper
{
	public static async Task InitializeAsync(IServiceProvider services)
	{
		using var scope = services.CreateScope();

		var roleManager = scope.ServiceProvider
			.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

		await RoleSeeder.SeedAsync(roleManager);
	}
}
