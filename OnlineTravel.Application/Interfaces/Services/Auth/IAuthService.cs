using OnlineTravel.Application.Features.Auth.Email;
using OnlineTravel.Application.Features.Auth.Login;
using OnlineTravel.Application.Features.Auth.Password;
using OnlineTravel.Application.Features.Auth.Register;

namespace OnlineTravel.Application.Interfaces.Services.Auth
{
	public interface IAuthService
	{
		Task<RegisterResponse> RegisterAsync(RegisterRequest request);
		Task<AuthResponse> LoginAsync(LoginRequest request);
		Task<ConfirmEmailResponse> ConfirmEmailAsync(string userId, string token);
		Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request);
		Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request);
		Task<AuthResponse> GoogleLoginAsync(string email);
		Task<AuthResponse> DeleteAccountAsync(Guid userId, string password);
		Task<string> RestoreAccountAsync(string email, string password);
		AuthResponse Logout();


	}
}
