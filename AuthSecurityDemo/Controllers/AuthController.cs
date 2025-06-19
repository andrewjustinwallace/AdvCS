using Microsoft.AspNetCore.Mvc;
using AuthSecurityDemo.Models;
using AuthSecurityDemo.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace AuthSecurityDemo.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken] // CSRF Protection
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _authService.LoginAsync(model.Email, model.Password);

        if (result.Success)
        {
            // Store JWT token in HTTP-only cookie for better security
            Response.Cookies.Append("AuthToken", result.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Use HTTPS in production
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(24)
            });

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", result.Message);
        return View(model);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken] // CSRF Protection
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _authService.RegisterAsync(model);

        if (result.Success)
        {
            // Store JWT token in HTTP-only cookie
            Response.Cookies.Append("AuthToken", result.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Use HTTPS in production
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(24)
            });

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", result.Message);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken] // CSRF Protection
    public IActionResult Logout()
    {
        Response.Cookies.Delete("AuthToken");
        TempData["SuccessMessage"] = "You have been logged out successfully.";
        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    [HttpGet]
    public IActionResult Profile()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var name = User.FindFirst(ClaimTypes.Name)?.Value;
        var age = User.FindFirst("age")?.Value;
        var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

        var profileData = new
        {
            UserId = userId,
            Email = email,
            Name = name,
            Age = age,
            Roles = roles
        };

        return View(profileData);
    }
}
