using AuthSecurityDemo.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Data;
using Dapper;

namespace AuthSecurityDemo.Services;

public class AuthService : IAuthService
{
    private readonly IDbConnection _connection;
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;

    public AuthService(IDbConnection connection, IConfiguration configuration, IUserService userService)
    {
        _connection = connection;
        _configuration = configuration;
        _userService = userService;
    }

    public async Task<AuthResult> LoginAsync(string email, string password)
    {
        try
        {
            var user = await _userService.GetUserByEmailAsync(email);
            
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "Invalid email or password"
                };
            }

            // Get user roles
            user.Roles = await _userService.GetUserRolesAsync(user.Id);

            var token = GenerateJwtToken(user);

            return new AuthResult
            {
                Success = true,
                Token = token,
                User = user,
                Message = "Login successful"
            };
        }
        catch (Exception ex)
        {
            return new AuthResult
            {
                Success = false,
                Message = $"Login failed: {ex.Message}"
            };
        }
    }

    public async Task<AuthResult> RegisterAsync(RegisterModel model)
    {
        try
        {
            // Check if user already exists
            var existingUser = await _userService.GetUserByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "User with this email already exists"
                };
            }

            // Hash password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            // Insert user
            var sql = @"
                INSERT INTO Users (Email, PasswordHash, FirstName, LastName, Age) 
                OUTPUT INSERTED.Id
                VALUES (@Email, @PasswordHash, @FirstName, @LastName, @Age)";

            var userId = await _connection.QuerySingleAsync<int>(sql, new
            {
                Email = model.Email,
                PasswordHash = passwordHash,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Age = model.Age
            });

            // Assign default "User" role
            await AssignRoleAsync(userId, "User");

            var user = await _userService.GetUserByIdAsync(userId);
            if (user != null)
            {
                user.Roles = await _userService.GetUserRolesAsync(userId);
                var token = GenerateJwtToken(user);

                return new AuthResult
                {
                    Success = true,
                    Token = token,
                    User = user,
                    Message = "Registration successful"
                };
            }

            return new AuthResult
            {
                Success = false,
                Message = "Registration failed"
            };
        }
        catch (Exception ex)
        {
            return new AuthResult
            {
                Success = false,
                Message = $"Registration failed: {ex.Message}"
            };
        }
    }

    public async Task<bool> AssignRoleAsync(int userId, string roleName)
    {
        try
        {
            var sql = @"
                INSERT INTO UserRoles (UserId, RoleId)
                SELECT @UserId, Id FROM Roles WHERE Name = @RoleName
                AND NOT EXISTS (
                    SELECT 1 FROM UserRoles ur 
                    INNER JOIN Roles r ON ur.RoleId = r.Id 
                    WHERE ur.UserId = @UserId AND r.Name = @RoleName
                )";

            var result = await _connection.ExecuteAsync(sql, new { UserId = userId, RoleName = roleName });
            return result > 0;
        }
        catch
        {
            return false;
        }
    }

    public string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new(ClaimTypes.GivenName, user.FirstName),
            new(ClaimTypes.Surname, user.LastName),
            new("age", user.Age.ToString()),
            new("user_id", user.Id.ToString())
        };

        // Add role claims
        foreach (var role in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(_configuration.GetValue<int>("Jwt:ExpiryInHours")),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
