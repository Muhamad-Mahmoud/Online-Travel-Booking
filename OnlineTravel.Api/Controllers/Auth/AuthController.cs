using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using OnlineTravel.Application.Features.Auth.Account;
using OnlineTravel.Application.Features.Auth.Login;
using OnlineTravel.Application.Features.Auth.Password;
using OnlineTravel.Application.Features.Auth.Register;
using OnlineTravel.Application.Interfaces.Services.Auth;

namespace OnlineTravel.Api.Controllers.Auth
{
	[ApiController]
	[Route("api/v1/auth")]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;
		private readonly IConfiguration _configuration;

		public AuthController(IAuthService authService, IConfiguration configuration)
		{
			_authService = authService;
			_configuration = configuration;
		}

		/// <summary>
		/// Register a new user account.
		/// </summary>
		[HttpPost("register")]
		public async Task<IActionResult> Register(
			[FromBody] RegisterRequest request)
		{
			var result = await _authService.RegisterAsync(request);

			if (!result.IsSuccess)
				return BadRequest(result);

			return Ok(result);
		}

		/// <summary>
		/// Authenticate a user and return a JWT token.
		/// </summary>
		[HttpPost("login")]
		public async Task<IActionResult> Login(
			[FromBody] LoginRequest request)
		{
			var result = await _authService.LoginAsync(request);

			if (!result.IsSuccess)
				return Unauthorized(result);

			return Ok(result);
		}

		/// <summary>
		/// Confirm a user's email address using a token.
		/// </summary>
		[HttpGet("confirm-email")]
		public async Task<IActionResult> ConfirmEmail([FromQuery] string userId,
													  [FromQuery] string token)
		{
			var result = await _authService.ConfirmEmailAsync(userId, token);

			if (!result.IsSuccess)
				return BadRequest(result);

			return Ok(result);
		}

		/// <summary>
		/// Send a password reset link to a user's email.
		/// </summary>
		[HttpPost("forgot-password")]
		public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
		{
			var result = await _authService.ForgotPasswordAsync(request);

			if (!result.IsSuccess)
				return BadRequest(result);

			return Ok(result);
		}

		/// <summary>
		/// Reset a user's password using a reset token.
		/// </summary>
		[HttpPost("reset-password")]
		public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
		{
			var result = await _authService.ResetPasswordAsync(request);

			if (!result.IsSuccess)
				return BadRequest(result);

			return Ok(result);
		}

		/// <summary>
		/// Initiate Google OAuth2 login flow.
		/// </summary>
		[HttpGet("google-login")]
		public IActionResult GoogleLogin()
		{
			var properties = new AuthenticationProperties
			{
				RedirectUri = Url.Action("GoogleResponse")
			};

			return Challenge(properties, "Google");
		}

		/// <summary>
		/// Handle the callback from Google OAuth2.
		/// </summary>
		[HttpGet("google-response")]
		public async Task<IActionResult> GoogleResponse()
		{
			var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);

			if (!result.Succeeded)
				return BadRequest(new { Message = "Google authentication failed" });

			var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;

			if (string.IsNullOrEmpty(email))
				return BadRequest(new { Message = "Email claim not found from Google" });

			var response = await _authService.GoogleLoginAsync(email);

			if (!response.IsSuccess)
				return BadRequest(response);

			var frontendUrl = _configuration["AppSettings:FrontendBaseUrl"];

			return Redirect($"{frontendUrl}/auth/google-success#token={response.Token}");
		}

		/// <summary>
		/// Permanently delete the authenticated user's account.
		/// </summary>
		[Authorize]
		[HttpDelete("delete-account")]
		public async Task<IActionResult> DeleteAccount([FromBody] DeleteAccountRequest request)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (string.IsNullOrEmpty(userId))
				return Unauthorized();

			var result = await _authService.DeleteAccountAsync(Guid.Parse(userId), request.Password);

			if (!result.IsSuccess)
				return BadRequest(result.Message);

			return Ok(result.Message);
		}

		/// <summary>
		/// Restore a previously deleted account.
		/// </summary>
		[HttpPost("restore-account")]
		public async Task<IActionResult> RestoreAccount([FromBody] RestoreAccountRequest request)
		{
			var result = await _authService.RestoreAccountAsync(request.Email, request.Password);

			if (result != "Account restored successfully")
				return BadRequest(result);

			return Ok(result);
		}


		/// <summary>
		/// List of current user's session logout.
		/// </summary>
		[Authorize]
		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
		{
			var result = await Task.FromResult(_authService.Logout());
			return Ok(result);
		}



	}
}


