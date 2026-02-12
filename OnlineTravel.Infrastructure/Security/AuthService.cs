using AutoMapper;
using Microsoft.AspNetCore.Identity;
using OnlineTravel.Application.Features.Auth.Dtos;
using OnlineTravel.Application.Features.Auth.Login;
using OnlineTravel.Application.Features.Auth.Register;
using OnlineTravel.Application.Interfaces.Services.Auth;
using OnlineTravel.Domain.Entities.Users;
using OnlineTravel.Infrastructure.Security.Jwt;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;


    public AuthService(
        UserManager<AppUser> userManager,
        IJwtService jwtService,
        IMapper mapper
        )
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _mapper = mapper;
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
    {
        if (await _userManager.FindByEmailAsync(request.Email) != null)
        {
            return new RegisterResponse
            {
                IsSuccess = false,
                Message = "Email already exists"
            };
        }

        var user = _mapper.Map<AppUser>(request);

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return new RegisterResponse
            {
                IsSuccess = false,
                Message = string.Join(", ",
                    result.Errors.Select(e => e.Description))
            };
        }

        await _userManager.AddToRoleAsync(user, "User");

        var jwt = await _jwtService.GenerateToken(user);

        return new RegisterResponse
        {
            IsSuccess = true,
            Message = "Registered successfully",
            Token = jwt.Token,
            ExpiresAt = jwt.ExpiresAt,
            User = _mapper.Map<UserDto>(user)
        };
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            return new LoginResponse
            {
                IsSuccess = false,
                Message = "Invalid email or password"
            };
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(
            user, request.Password);

        if (!isPasswordValid)
        {
            return new LoginResponse
            {
                IsSuccess = false,
                Message = "Invalid email or password"
            };
        }

        var jwt = await _jwtService.GenerateToken(user);

        return new LoginResponse
        {
            IsSuccess = true,
            Token = jwt.Token,
            ExpiresAt = jwt.ExpiresAt,
            Message = "Login successful"
        };
    }

}

