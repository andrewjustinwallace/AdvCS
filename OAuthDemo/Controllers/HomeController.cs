using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace OAuthDemo.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [Authorize]
    public IActionResult Dashboard()
    {
        ViewBag.UserName = User.Identity?.Name;
        ViewBag.Provider = User.FindFirst("provider")?.Value;
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}