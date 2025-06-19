using AuthSecurityDemo.Models;
using System.Security.Claims;

namespace AuthSecurityDemo.Services;

public interface IAuthService
{
    Task<AuthResult> LoginAsync(string email, string password);
    Task<AuthResult> RegisterAsync(RegisterModel model);
    Task<bool> AssignRoleAsync(int userId, string roleName);
    string GenerateJwtToken(User user);
}

public interface IUserService
{
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetUserByEmailAsync(string email);
    Task<List<User>> GetAllUsersAsync();
    Task<List<Comment>> GetCommentsAsync();
    Task<bool> AddCommentAsync(int userId, string content);
    Task<List<string>> GetUserRolesAsync(int userId);
}
