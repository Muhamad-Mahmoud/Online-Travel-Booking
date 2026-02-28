using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using OnlineTravel.Application.Features.Auth;
using OnlineTravel.Application.Features.Auth.Login;
using OnlineTravel.Application.Interfaces.Services.Auth;
using OnlineTravel.Mvc.Models;
using OnlineTravel.Mvc.Models.Account;

namespace OnlineTravel.Mvc.Controllers;

[AllowAnonymous]
public class AccountController(IAuthService authService) : BaseController
{

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Admin");
        }

        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await authService.LoginAsync(new LoginRequest
        {
            Email = model.Email,
            Password = model.Password
        });

        if (result.IsSuccess)
        {
            // Check if user is Admin
            var isAdmin = result.User?.Roles.Any(r => r.Equals("Admin", StringComparison.OrdinalIgnoreCase)) ?? false;

            if (!isAdmin)
            {
                ModelState.AddModelError(string.Empty, "Access denied. Only administrators can log in here.");
                return View(model);
            }

            // Create Claims and Sign In
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, result.User?.Name ?? result.User?.Email ?? ""),
                new(ClaimTypes.Email, result.User?.Email ?? ""),
                new(ClaimTypes.NameIdentifier, result.User?.Id.ToString() ?? ""),
                new("Token", result.Token ?? "")
            };

            if (result.User?.Roles != null)
            {
                foreach (var role in result.User.Roles)
                {
                    claims.Add(new(ClaimTypes.Role, role));
                }
            }

            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = result.ExpiresAt
            };

            await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(claimsIdentity), authProperties);

            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            return RedirectToAction("Index", "Dashboard");
        }

        ModelState.AddModelError(string.Empty, result.Message ?? "Invalid login attempt.");
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("Cookies");
        return RedirectToAction("Login", "Account");
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}

