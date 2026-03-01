using Microsoft.OpenApi.Models;

namespace OnlineTravel.Api.Extensions
{
	public static class CustomJwtAuthExtention
	{



		public static void AddSwaggerGenJwtAuth(this IServiceCollection services)
		{
			services.AddSwaggerGen(options =>
			{
				// Use unified request names (replace internal "Command" with public "Request")
				options.CustomSchemaIds(type => type.Name.Replace("Command", "Request"));

				// Order endpoints by business flow, then by HTTP method
				options.OrderActionsBy(apiDesc =>
				{
					var controllerOrder = new Dictionary<string, int>
					{
						{ "Auth", 1 },
						{ "Bookings", 2 },
						{ "Payments", 3 },
						{ "Flights", 4 },
						{ "Hotels", 5 },
						{ "Tours", 6 },
						{ "TourReviews", 7 },
						{ "Cars", 8 },
						{ "CarBrands", 9 },
						{ "CarPricingTiers", 10 },
						{ "Carriers", 11 },
						{ "Airports", 12 },
						{ "Categories", 13 },
						{ "Favorites", 14 },
						{ "AdminDashboard", 15 },
						{ "Upload", 16 },
						{ "Dev", 17 },
					};

					var methodOrder = new Dictionary<string, int>
					{
						{ "POST", 1 },
						{ "GET", 2 },
						{ "PUT", 3 },
						{ "PATCH", 4 },
						{ "DELETE", 5 },
					};

					var controller = apiDesc.ActionDescriptor.RouteValues["controller"] ?? "";
					var action = apiDesc.ActionDescriptor.RouteValues["action"] ?? "";
					var method = apiDesc.HttpMethod ?? "GET";

					var cOrder = controllerOrder.TryGetValue(controller, out var co) ? co : 999;
					var mOrder = methodOrder.TryGetValue(method, out var mo) ? mo : 9;

					return $"{cOrder:D3}_{mOrder}_{action}";
				});

				var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				options.IncludeXmlComments(xmlPath);

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
