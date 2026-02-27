using Microsoft.AspNetCore.Identity;

namespace OnlineTravel.Infrastructure.Identity;

public static class RoleSeeder
{
	public static async Task SeedAsync(RoleManager<IdentityRole<Guid>> roleManager)
	{
		string[] roles = ["Admin", "User"];

		foreach (var roleName in roles)
		{
			var exists = await roleManager.RoleExistsAsync(roleName);

			if (!exists)
			{
				var role = new IdentityRole<Guid>
				{
					Name = roleName
				};

				await roleManager.CreateAsync(role);
			}
		}
	}
}

