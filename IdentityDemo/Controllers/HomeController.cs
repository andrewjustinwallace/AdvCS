using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace IdentityDemo.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [Authorize]
    public IActionResult Dashboard()
    {
        var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
        ViewBag.Claims = claims;
        return View();
    }

    [Authorize(Roles = "Admin")]
    public IActionResult AdminOnly()
    {
        return View();
    }

    [Authorize(Policy = "MinimumAge18")]
    public IActionResult AdultContent()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}