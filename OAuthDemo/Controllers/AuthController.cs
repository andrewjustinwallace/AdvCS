using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using OAuthDemo.Models;
using System.Security.Claims;

namespace OAuthDemo.Controllers;

public class AuthController : Controller
{
    [HttpGet]
    public IActionResult Login()
    {
        var providers = new List<AuthenticationProvider>
        {
            new() { Name = "Google", DisplayName = "Google", Icon = "fab fa-google" },
            new() { Name = "Microsoft", DisplayName = "Microsoft", Icon = "fab fa-microsoft" },
            new() { Name = "Facebook", DisplayName = "Facebook", Icon = "fab fa-facebook" }
        };
        
        return View(providers);
    }

    [HttpPost]
    public IActionResult ExternalLogin(string provider, string returnUrl = "/")
    {
        var redirectUrl = Url.Action("ExternalLoginCallback", "Auth", new { returnUrl });
        var properties = new AuthenticationProperties
        {
            RedirectUri = redirectUrl
        };
        
        return Challenge(properties, provider);
    }

    [HttpGet]
    public async Task<IActionResult> ExternalLoginCallback(string returnUrl = "/")
    {
        var result = await HttpContext.AuthenticateAsync("External");
        
        if (!result.Succeeded || result.Principal == null)
        {
            TempData["Error"] = "External authentication failed.";
            return RedirectToAction("Login");
        }

        // Extract claims from external provider
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, result.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? ""),
            new(ClaimTypes.Name, result.Principal.FindFirst(ClaimTypes.Name)?.Value ?? ""),
            new(ClaimTypes.Email, result.Principal.FindFirst(ClaimTypes.Email)?.Value ?? ""),
            new(ClaimTypes.GivenName, result.Principal.FindFirst(ClaimTypes.GivenName)?.Value ?? ""),
            new(ClaimTypes.Surname, result.Principal.FindFirst(ClaimTypes.Surname)?.Value ?? ""),
            new("provider", result.Principal.FindFirst("provider")?.Value ?? ""),
            new("picture", result.Principal.FindFirst("picture")?.Value ?? "")
        };

        // Add additional provider-specific claims
        foreach (var claim in result.Principal.Claims)
        {
            if (!claims.Any(c => c.Type == claim.Type))
            {
                claims.Add(new Claim(claim.Type, claim.Value));
            }
        }

        var identity = new ClaimsIdentity(claims, "External");
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync("Cookies", principal);
        
        return LocalRedirect(returnUrl);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("Cookies");
        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    public IActionResult Profile()
    {
        var profile = new UserProfile
        {
            Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "",
            Email = User.FindFirst(ClaimTypes.Email)?.Value ?? "",
            Name = User.FindFirst(ClaimTypes.Name)?.Value ?? "",
            FirstName = User.FindFirst(ClaimTypes.GivenName)?.Value ?? "",
            LastName = User.FindFirst(ClaimTypes.Surname)?.Value ?? "",
            Picture = User.FindFirst("picture")?.Value ?? "",
            Provider = User.FindFirst("provider")?.Value ?? "",
            EmailVerified = bool.Parse(User.FindFirst("email_verified")?.Value ?? "false"),
            Claims = User.Claims.Select(c => new ClaimInfo { Type = c.Type, Value = c.Value }).ToList()
        };

        return View(profile);
    }
}