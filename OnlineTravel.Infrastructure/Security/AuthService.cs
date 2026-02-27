using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using OnlineTravel.Application.Features.Auth.ConfirmEmail;
using OnlineTravel.Application.Features.Auth.Dtos;
using OnlineTravel.Application.Features.Auth.Login;
using OnlineTravel.Application.Features.Auth.Password;
using OnlineTravel.Application.Features.Auth.Register;
using OnlineTravel.Application.Interfaces.Services.Auth;
using OnlineTravel.Domain.Entities.Users;
using OnlineTravel.Domain.Enums;
using OnlineTravel.Infrastructure.Security.Jwt;

public class AuthService : IAuthService
{
	private readonly UserManager<AppUser> _userManager;
	private readonly IJwtService _jwtService;
	private readonly IMapper _mapper;
	private readonly IEmailService _emailService;
	private readonly IConfiguration _configuration;

	public AuthService(
		UserManager<AppUser> userManager,
		IJwtService jwtService,
		IMapper mapper,
		IEmailService emailService,
		IConfiguration configuration)
	{
		_userManager = userManager;
		_jwtService = jwtService;
		_mapper = mapper;
		_emailService = emailService;
		_configuration = configuration;
	}

	// REGISTER
	public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
	{
		if (await _userManager.FindByEmailAsync(request.Email) != null)
			return new RegisterResponse { IsSuccess = false, Message = "Email already exists" };

		var user = _mapper.Map<AppUser>(request);

		var result = await _userManager.CreateAsync(user, request.Password);

		if (!result.Succeeded)
			return new RegisterResponse
			{
				IsSuccess = false,
				Message = string.Join(", ", result.Errors.Select(e => e.Description))
			};

		await _userManager.AddToRoleAsync(user, "User");

		var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
		var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

		var frontendUrl = _configuration["AppSettings:FrontendBaseUrl"];
		var apiBaseUrl = _configuration["AppSettings:ApiBaseUrl"];
		var useApiForTest = _configuration.GetValue<bool>("AppSettings:UseApiForConfirmation");

		if (string.IsNullOrEmpty(frontendUrl) && !useApiForTest)
			return new RegisterResponse { IsSuccess = false, Message = "Base URL is not configured" };

		var confirmationLink = useApiForTest
			? $"{apiBaseUrl}/api/auth/confirm-email?userId={user.Id}&token={encodedToken}"
			: $"{frontendUrl}/confirm-email?userId={user.Id}&token={encodedToken}";

		await _emailService.SendEmailAsync(
			user.Email!,
			"Confirm your email",
			$"<a href='{confirmationLink}'>Confirm Email</a>"
		);

		return new RegisterResponse
		{
			IsSuccess = true,
			Message = "Registered successfully. Please check your email."
		};
	}

	// LOGIN
	public async Task<AuthResponse> LoginAsync(LoginRequest request)
	{
		var user = await _userManager.FindByEmailAsync(request.Email);

		if (user == null || user.Status == UserStatus.Deleted)
			return new AuthResponse { IsSuccess = false, Message = "Unauthorized" };

		if (!await _userManager.CheckPasswordAsync(user, request.Password))
			return new AuthResponse { IsSuccess = false, Message = "Invalid email or password" };

		if (!await _userManager.IsEmailConfirmedAsync(user))
			return new AuthResponse { IsSuccess = false, Message = "Please confirm your email first" };

		var jwt = await _jwtService.GenerateToken(user);
		var userDto = _mapper.Map<UserDto>(user);
		userDto.Roles = (await _userManager.GetRolesAsync(user)).ToList();

		return new AuthResponse
		{
			IsSuccess = true,
			Token = jwt.Token,
			ExpiresAt = jwt.ExpiresAt,
			Message = "Login successful",
			User = userDto
		};
	}

	// CONFIRM EMAIL
	public async Task<ConfirmEmailResponse> ConfirmEmailAsync(string userId, string token)
	{
		if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
			return new ConfirmEmailResponse { IsSuccess = false, Message = "Invalid request" };

		var user = await _userManager.FindByIdAsync(userId);

		if (user == null || user.Status == UserStatus.Deleted)
			return new ConfirmEmailResponse { IsSuccess = false, Message = "Unauthorized" };

		var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
		var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

		if (!result.Succeeded)
			return new ConfirmEmailResponse { IsSuccess = false, Message = "Email confirmation failed" };

		return new ConfirmEmailResponse { IsSuccess = true, Message = "Email confirmed successfully" };
	}

	// FORGOT PASSWORD
	public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request)
	{
		var user = await _userManager.FindByEmailAsync(request.Email);

		if (user == null || user.Status == UserStatus.Deleted ||
			!await _userManager.IsEmailConfirmedAsync(user))
		{
			return new ForgotPasswordResponse
			{
				IsSuccess = true,
				Message = "If this email exists, a reset link has been sent."
			};
		}

		var token = await _userManager.GeneratePasswordResetTokenAsync(user);
		var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

		var frontendUrl = _configuration["AppSettings:FrontendBaseUrl"];
		var apiBaseUrl = _configuration["AppSettings:ApiBaseUrl"];
		var useApiForTest = _configuration.GetValue<bool>("AppSettings:UseApiForConfirmation");

		if (string.IsNullOrEmpty(frontendUrl) && !useApiForTest)
			return new ForgotPasswordResponse { IsSuccess = false, Message = "Base URL not configured" };

		var resetLink = useApiForTest
			? $"{apiBaseUrl}/api/auth/reset-password?email={user.Email}&token={encodedToken}"
			: $"{frontendUrl}/reset-password?email={user.Email}&token={encodedToken}";

		await _emailService.SendEmailAsync(
			user.Email!,
			"Reset your password",
			$"<a href='{resetLink}'>Reset Password</a>"
		);

		return new ForgotPasswordResponse
		{
			IsSuccess = true,
			Message = "If this email exists, a reset link has been sent."
		};
	}

	// RESET PASSWORD
	public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request)
	{
		var user = await _userManager.FindByEmailAsync(request.Email);

		if (user == null || user.Status == UserStatus.Deleted)
			return new ResetPasswordResponse { IsSuccess = false, Message = "Unauthorized" };

		var decodedToken =
			Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));

		var result = await _userManager.ResetPasswordAsync(
			user,
			decodedToken,
			request.NewPassword);

		if (!result.Succeeded)
			return new ResetPasswordResponse
			{
				IsSuccess = false,
				Message = string.Join(", ", result.Errors.Select(e => e.Description))
			};

		return new ResetPasswordResponse
		{
			IsSuccess = true,
			Message = "Password has been reset successfully"
		};
	}

	// GOOGLE LOGIN
	public async Task<AuthResponse> GoogleLoginAsync(string email)
	{
		if (string.IsNullOrEmpty(email))
			return new AuthResponse { IsSuccess = false, Message = "Unauthorized" };

		var user = await _userManager.FindByEmailAsync(email);

		if (user != null && user.Status == UserStatus.Deleted)
			return new AuthResponse { IsSuccess = false, Message = "Unauthorized" };

		if (user == null)
		{
			user = new AppUser
			{
				Email = email,
				UserName = email,
				EmailConfirmed = true
			};

			var createResult = await _userManager.CreateAsync(user);

			if (!createResult.Succeeded)
				return new AuthResponse { IsSuccess = false, Message = "User creation failed" };
		}

		var jwt = await _jwtService.GenerateToken(user);
		var userDto = _mapper.Map<UserDto>(user);
		userDto.Roles = (await _userManager.GetRolesAsync(user)).ToList();

		return new AuthResponse
		{
			IsSuccess = true,
			Token = jwt.Token,
			ExpiresAt = jwt.ExpiresAt,
			Message = "Google login successful",
			User = userDto
		};
	}

	// DELETE ACCOUNT
	public async Task<AuthResponse> DeleteAccountAsync(Guid userId, string password)
	{
		var user = await _userManager.FindByIdAsync(userId.ToString());

		if (user == null || user.Status == UserStatus.Deleted)
			return new AuthResponse { IsSuccess = false, Message = "Unauthorized" };

		if (!await _userManager.CheckPasswordAsync(user, password))
			return new AuthResponse { IsSuccess = false, Message = "Invalid credentials" };

		user.Status = UserStatus.Deleted;
		user.DeletedAt = DateTime.UtcNow;

		var result = await _userManager.UpdateAsync(user);

		if (!result.Succeeded)
			return new AuthResponse { IsSuccess = false, Message = "Failed to delete account" };

		return new AuthResponse
		{
			IsSuccess = true,
			Message = "Account deleted successfully."
		};
	}

	// RESTORE ACCOUNT
	public async Task<string> RestoreAccountAsync(string email, string password)
	{
		var user = await _userManager.FindByEmailAsync(email);

		if (user == null)
			return "Unauthorized";

		if (user.Status != UserStatus.Deleted)
			return "Account is not deleted";

		if (!await _userManager.CheckPasswordAsync(user, password))
			return "Invalid credentials";

		if (user.DeletedAt.HasValue &&
			user.DeletedAt.Value.AddDays(30) < DateTime.UtcNow)
			return "Restore period expired";

		user.Status = UserStatus.Active;
		user.DeletedAt = null;

		await _userManager.UpdateAsync(user);

		return "Account restored successfully";
	}

	// LOGOUT
	public AuthResponse Logout()
	{
		return new AuthResponse
		{
			IsSuccess = true,
			Message = "Logged out successfully"
		};
	}
}
