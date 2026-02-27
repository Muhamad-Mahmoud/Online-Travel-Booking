using Serilog;

namespace OnlineTravel.Api.Extensions;

public static class WebApplicationBuilderExtensions
{
	public static void ConfigureSerilog(this WebApplicationBuilder builder)
	{
		builder.Host.UseSerilog((context, configuration) =>
			configuration.ReadFrom.Configuration(context.Configuration));
	}
}

