using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnlineTravel.Application.Features.Auth.Dtos;
using OnlineTravel.Domain.Entities.Users;

namespace OnlineTravel.Infrastructure.Security.Jwt
{
	public class JwtService : IJwtService
	{
		private readonly JwtOptions _options;
		private readonly UserManager<AppUser> _userManager;

		public JwtService(
			IOptions<JwtOptions> options,
			UserManager<AppUser> userManager)
		{
			_options = options.Value;
			_userManager = userManager;
		}

		public async Task<JwtResult> GenerateToken(AppUser user)
		{
			var roles = await _userManager.GetRolesAsync(user);

			var claims = new List<Claim>
						{
							new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
							new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
							new Claim(ClaimTypes.Email, user.Email!),
							new Claim(ClaimTypes.Name, user.UserName!)
						};

			foreach (var role in roles)
				claims.Add(new Claim(ClaimTypes.Role, role));

			var key = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(_options.Key));

			var expiresAt = DateTime.UtcNow.AddDays(_options.DurationInDays);

			var token = new JwtSecurityToken(
				issuer: _options.Issuer,
				audience: _options.Audience,
				claims: claims,
				expires: expiresAt,
				signingCredentials: new SigningCredentials(
				key, SecurityAlgorithms.HmacSha256)
			);

			return new JwtResult
			{
				Token = new JwtSecurityTokenHandler().WriteToken(token),
				ExpiresAt = expiresAt
			};
		}
	}
}
