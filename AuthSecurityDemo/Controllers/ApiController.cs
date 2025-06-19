using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AuthSecurityDemo.Services;
using AuthSecurityDemo.Models;
using System.Security.Claims;

namespace AuthSecurityDemo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApiController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public ApiController(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<object>>> Login([FromBody] LoginModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Invalid model data"
            });
        }

        var result = await _authService.LoginAsync(model.Email, model.Password);

        if (result.Success)
        {
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = result.Message,
                Data = new { Token = result.Token, User = new { result.User!.Id, result.User.Email, result.User.FirstName } }
            });
        }

        return Unauthorized(new ApiResponse<object>
        {
            Success = false,
            Message = result.Message
        });
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<object>>> Register([FromBody] RegisterModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Invalid model data"
            });
        }

        var result = await _authService.RegisterAsync(model);

        if (result.Success)
        {
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = result.Message,
                Data = new { Token = result.Token, User = new { result.User!.Id, result.User.Email, result.User.FirstName } }
            });
        }

        return BadRequest(new ApiResponse<object>
        {
            Success = false,
            Message = result.Message
        });
    }

    [Authorize]
    [HttpGet("profile")]
    public ActionResult<ApiResponse<object>> GetProfile()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var name = User.FindFirst(ClaimTypes.Name)?.Value;
        var age = User.FindFirst("age")?.Value;
        var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Profile retrieved successfully",
            Data = new
            {
                UserId = userId,
                Email = email,
                Name = name,
                Age = age,
                Roles = roles,
                Claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList()
            }
        });
    }

    [Authorize]
    [HttpGet("comments")]
    public async Task<ActionResult<ApiResponse<List<Comment>>>> GetComments()
    {
        var comments = await _userService.GetCommentsAsync();
        
        return Ok(new ApiResponse<List<Comment>>
        {
            Success = true,
            Message = "Comments retrieved successfully",
            Data = comments
        });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("users")]
    public async Task<ActionResult<ApiResponse<List<User>>>> GetUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        
        // Remove sensitive data
        foreach (var user in users)
        {
            user.PasswordHash = "[HIDDEN]";
        }

        return Ok(new ApiResponse<List<User>>
        {
            Success = true,
            Message = "Users retrieved successfully",
            Data = users
        });
    }

    [Authorize(Policy = "MinimumAge18")]
    [HttpGet("restricted")]
    public ActionResult<ApiResponse<string>> GetRestrictedContent()
    {
        return Ok(new ApiResponse<string>
        {
            Success = true,
            Message = "Access granted to restricted content",
            Data = "This content is only available to users 18 and older."
        });
    }

    // Demonstrate various claim-based authorization
    [Authorize]
    [HttpGet("claims-demo")]
    public ActionResult<ApiResponse<object>> ClaimsDemo()
    {
        var claimsData = new
        {
            AllClaims = User.Claims.Select(c => new { c.Type, c.Value }).ToList(),
            SpecificClaims = new
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Email = User.FindFirst(ClaimTypes.Email)?.Value,
                Name = User.FindFirst(ClaimTypes.Name)?.Value,
                GivenName = User.FindFirst(ClaimTypes.GivenName)?.Value,
                Surname = User.FindFirst(ClaimTypes.Surname)?.Value,
                Age = User.FindFirst("age")?.Value,
                Roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList()
            },
            AuthenticationInfo = new
            {
                IsAuthenticated = User.Identity?.IsAuthenticated ?? false,
                AuthenticationType = User.Identity?.AuthenticationType,
                Name = User.Identity?.Name
            }
        };

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Claims information retrieved",
            Data = claimsData
        });
    }
}
