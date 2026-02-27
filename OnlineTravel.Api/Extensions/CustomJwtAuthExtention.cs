using Microsoft.OpenApi.Models;

namespace OnlineTravel.Api.Extensions
{
	public static class CustomJwtAuthExtention
	{



		public static void AddSwaggerGenJwtAuth(this IServiceCollection services)
		{
			services.AddSwaggerGen(options =>
			{
				// Use fully qualified type names to avoid schema ID collisions
				// (e.g., Create.MoneyFormModel vs Update.MoneyFormModel).
				options.CustomSchemaIds(type => (type.FullName ?? type.Name).Replace("+", "."));

				options.SwaggerDoc("v1", new OpenApiInfo
				{
					Version = "v1",
					Title = "Online Travel Booking API",
					Description = "A robust backend service designed to manage online travel bookings (Flights, Hotels, Cars, Tours).",
					Contact = new OpenApiContact
					{
						Name = "Online Travel Support",
						Email = "support@onlinetravel.com"
					}
				});

				var securityScheme = new OpenApiSecurityScheme
				{
					Name = "Authorization",
					Type = SecuritySchemeType.Http,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Description = "Enter the JWT token in the format: Bearer {your token}",
					Reference = new OpenApiReference
					{
						Type = ReferenceType.SecurityScheme,
						Id = "Bearer"
					}
				};

				options.AddSecurityDefinition("Bearer", securityScheme);
				options.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{ securityScheme, new List<string>() }
			});
			});
		}
	}
}
