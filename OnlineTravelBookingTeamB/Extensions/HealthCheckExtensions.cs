using Microsoft.Extensions.Diagnostics.HealthChecks;
using OnlineTravel.Infrastructure.Persistence.Context;

namespace OnlineTravelBookingTeamB.Extensions;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddAppHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy())
            .AddDbContextCheck<OnlineTravelDbContext>();

        return services;
    }

}
