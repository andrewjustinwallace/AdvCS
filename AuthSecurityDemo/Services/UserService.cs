using AuthSecurityDemo.Models;
using System.Data;
using Dapper;
using System.Web;

namespace AuthSecurityDemo.Services;

public class UserService : IUserService
{
    private readonly IDbConnection _connection;

    public UserService(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        var sql = "SELECT * FROM Users WHERE Id = @Id";
        return await _connection.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var sql = "SELECT * FROM Users WHERE Email = @Email";
        return await _connection.QuerySingleOrDefaultAsync<User>(sql, new { Email = email });
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        var sql = "SELECT * FROM Users ORDER BY CreatedAt DESC";
        var users = await _connection.QueryAsync<User>(sql);
        return users.ToList();
    }

    public async Task<List<Comment>> GetCommentsAsync()
    {
        var sql = @"
            SELECT c.Id, c.UserId, c.Content, c.CreatedAt, 
                   CONCAT(u.FirstName, ' ', u.LastName) as UserName
            FROM Comments c
            INNER JOIN Users u ON c.UserId = u.Id
            ORDER BY c.CreatedAt DESC";

        var comments = await _connection.QueryAsync<Comment>(sql);
        return comments.ToList();
    }

    public async Task<bool> AddCommentAsync(int userId, string content)
    {
        try
        {
            // **IMPORTANT: Stored XSS Prevention**
            // HTML encode the content before storing in database
            var sanitizedContent = HttpUtility.HtmlEncode(content);

            var sql = @"
                INSERT INTO Comments (UserId, Content)
                VALUES (@UserId, @Content)";

            var result = await _connection.ExecuteAsync(sql, new { UserId = userId, Content = sanitizedContent });
            return result > 0;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<string>> GetUserRolesAsync(int userId)
    {
        var sql = @"
            SELECT r.Name
            FROM Roles r
            INNER JOIN UserRoles ur ON r.Id = ur.RoleId
            WHERE ur.UserId = @UserId";

        var roles = await _connection.QueryAsync<string>(sql, new { UserId = userId });
        return roles.ToList();
    }
}
