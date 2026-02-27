using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace OnlineTravel.Infrastructure.Security;

public static class JwtAuthenticationExtensions
{
	public static IServiceCollection AddJwtAuthentication(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddAuthentication()
		.AddJwtBearer(options =>
		{
			options.RequireHttpsMetadata = false;
			options.SaveToken = false;

			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,

				ValidIssuer = configuration["Jwt:Issuer"],
				ValidAudience = configuration["Jwt:Audience"],

				IssuerSigningKey = new SymmetricSecurityKey(
					Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)
				)
			};
		});

		services.AddAuthorization();

		return services;
	}
}

