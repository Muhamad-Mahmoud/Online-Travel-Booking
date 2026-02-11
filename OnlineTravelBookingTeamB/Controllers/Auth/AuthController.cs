using Microsoft.AspNetCore.Mvc;
using OnlineTravel.Application.Features.Auth.Register;
using OnlineTravel.Application.Features.Auth.Login;
using OnlineTravel.Application.Interfaces.Services.Auth;

namespace OnlineTravel.API.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
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
    }
}

