using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

		[HttpPost("register")]
		public async Task<IActionResult> Register(
			[FromBody] RegisterRequest request)
		{
			var result = await _authService.RegisterAsync(request);

			if (!result.IsSuccess)
				return BadRequest(result);

			return Ok(result);
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(
			[FromBody] LoginRequest request)
		{
			var result = await _authService.LoginAsync(request);

			if (!result.IsSuccess)
				return Unauthorized(result);

			return Ok(result);
		}

		[HttpGet("confirm-email")]
		public async Task<IActionResult> ConfirmEmail([FromQuery] string userId,
													  [FromQuery] string token)
		{
			var result = await _authService.ConfirmEmailAsync(userId, token);

			if (!result.IsSuccess)
				return BadRequest(result);

			return Ok(result);
		}

		[HttpPost("forgot-password")]
		public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
		{
			var result = await _authService.ForgotPasswordAsync(request);

			if (!result.IsSuccess)
				return BadRequest(result);

			return Ok(result);
		}

		[HttpPost("reset-password")]
		public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
		{
			var result = await _authService.ResetPasswordAsync(request);

			if (!result.IsSuccess)
				return BadRequest(result);

			return Ok(result);
		}

		[HttpGet("google-login")]
		public IActionResult GoogleLogin()
		{
			var properties = new AuthenticationProperties
			{
				RedirectUri = Url.Action("GoogleResponse")
			};

			return Challenge(properties, "Google");
		}

		[HttpGet("google-response")]
		public async Task<IActionResult> GoogleResponse()
		{
			var result = await HttpContext.AuthenticateAsync("Google");

			if (!result.Succeeded)
				return BadRequest("Google authentication failed");

			var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;

			var response = await _authService.GoogleLoginAsync(email!);

			if (!response.IsSuccess)
				return BadRequest(response);

			var frontendUrl = _configuration["AppSettings:FrontendBaseUrl"];

			return Redirect($"{frontendUrl}/auth/google-success#token={response.Token}");


		}

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

		[HttpPost("restore-account")]
		public async Task<IActionResult> RestoreAccount([FromBody] RestoreAccountRequest request)
		{
			var result = await _authService.RestoreAccountAsync(request.Email, request.Password);

			if (result != "Account restored successfully")
				return BadRequest(result);

			return Ok(result);
		}


		[Authorize]
		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
		{
			var result = await Task.FromResult(_authService.Logout());
			return Ok(result);
		}



	}
}


