using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities.Users;
using OnlineTravel.Infrastructure.Persistence.Context;
using OnlineTravel.Infrastructure.Persistence.UnitOfWork;

namespace OnlineTravel.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Add DbContext with SQL Server
        AddDatabaseContext(services, configuration);

        // Add Identity
        services.AddIdentityCore<AppUser>(options =>
        {
            // Configure identity options if needed
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        })
        .AddRoles<IdentityRole<Guid>>()
        .AddEntityFrameworkStores<OnlineTravelDbContext>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

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