using Microsoft.AspNetCore.Mvc;
using AuthSecurityDemo.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using AuthSecurityDemo.Models;
using System.Web;

namespace AuthSecurityDemo.Controllers;

public class HomeController : Controller
{
    private readonly IUserService _userService;

    public HomeController(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        var comments = await _userService.GetCommentsAsync();
        return View(comments);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken] // CSRF Protection
    public async Task<IActionResult> AddComment(CommentModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Invalid comment content.";
            return RedirectToAction("Index");
        }

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out int userId))
        {
            TempData["ErrorMessage"] = "User not found.";
            return RedirectToAction("Index");
        }

        var success = await _userService.AddCommentAsync(userId, model.Content);

        if (success)
        {
            TempData["SuccessMessage"] = "Comment added successfully!";
        }
        else
        {
            TempData["ErrorMessage"] = "Failed to add comment.";
        }

        return RedirectToAction("Index");
    }

    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> AdminPanel()
    {
        var users = await _userService.GetAllUsersAsync();
        return View(users);
    }

    [Authorize(Policy = "ManagerOrAdmin")]
    public IActionResult ManagerArea()
    {
        return View();
    }

    [Authorize(Policy = "MinimumAge18")]
    public IActionResult RestrictedContent()
    {
        return View();
    }

    // Demonstrate XSS vulnerability (for educational purposes)
    public IActionResult XssDemo()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult XssDemo(string userInput)
    {
        // **REFLECTED XSS PREVENTION**: Always HTML encode user input
        ViewBag.SafeOutput = HttpUtility.HtmlEncode(userInput);
        
        // **DANGEROUS**: This would be vulnerable to reflected XSS
        // ViewBag.DangerousOutput = userInput; // Never do this!
        
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Error()
    {
        return View();
    }
}
