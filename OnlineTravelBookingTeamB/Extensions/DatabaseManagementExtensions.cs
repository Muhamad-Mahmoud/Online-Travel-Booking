using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineTravel.Domain.Entities.Users;
using OnlineTravel.Infrastructure.Persistence.Context;

namespace Microsoft.AspNetCore.Builder
{
    public static class DatabaseManagementExtensions
    {
        public static async Task ApplyDatabaseMigrationsAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    var context = services.GetRequiredService<OnlineTravelDbContext>();
                    var userManager = services.GetRequiredService<UserManager<AppUser>>();
                    //var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    await context.Database.MigrateAsync();

                    // Uncomment when seeding is needed
                    // await ApplicationDbContextSeed.SeedAsync(context, userManager, roleManager);
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "An error occurred during migration");
                }
            }
        }
    }
}
