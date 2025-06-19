# ASP.NET Core Identity Demo

This project demonstrates the implementation of ASP.NET Core Identity Framework with claims-based authentication.

## Key Features Demonstrated

### 1. Identity Framework Setup
- Custom `ApplicationUser` extending `IdentityUser`
- Entity Framework integration with SQLite
- Role-based and claims-based authorization

### 2. Authentication & Authorization
- **Cookie-based authentication** (no JWT needed)
- **Role-based authorization**: Admin, Manager, User roles
- **Claims-based authorization**: Age verification policy
- **Custom claims**: age, full_name, department

### 3. Key Differences from JWT Approach

| Feature | Identity Framework | JWT |
|---------|-------------------|-----|
| **State** | Stateful (server-side sessions) | Stateless |
| **Storage** | Database + encrypted cookies | Self-contained tokens |
| **Claims** | Stored in database, loaded on login | Embedded in token |
| **Updates** | Claims updated immediately | Requires new token |
| **Security** | HttpOnly cookies, CSRF protection | Bearer tokens |

## Project Structure

### Models
- `ApplicationUser` - Custom user entity with additional properties
- `ViewModels` - Registration, Login, and Profile view models

### Controllers
- `AccountController` - Registration, Login, Logout, Profile
- `HomeController` - Dashboard and demo pages with authorization

### Authorization Policies
```csharp
// Role-based
[Authorize(Roles = "Admin")]

// Claims-based  
[Authorize(Policy = "MinimumAge18")]
```

## Running the Project

1. **Restore packages**:
   ```bash
   dotnet restore
   ```

2. **Run the application**:
   ```bash
   dotnet run
   ```

3. **Test accounts**:
   - Admin: `admin@test.com` / `Admin123!`
   - Or register a new account

## Key Code Examples

### Custom User with Claims
```csharp
public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? Age { get; set; }
}
```

### Adding Claims on Registration
```csharp
await _userManager.AddClaimAsync(user, new Claim("age", model.Age?.ToString() ?? "0"));
await _userManager.AddClaimAsync(user, new Claim("full_name", $"{model.FirstName} {model.LastName}"));
```

### Claims-Based Authorization Policy
```csharp
options.AddPolicy("MinimumAge18", policy => 
    policy.RequireClaim("age", "18", "19", "20", /* ... */));
```

### Accessing Claims in Controllers
```csharp
var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
var roles = await _userManager.GetRolesAsync(user);
var claims = await _userManager.GetClaimsAsync(user);
```

## Benefits of Identity Framework

1. **Built-in User Management**: Registration, login, password reset, etc.
2. **Automatic Claims Population**: Claims automatically available in `User.Claims`
3. **Database Integration**: User data, roles, and claims stored persistently
4. **Security Features**: Password hashing, lockout, two-factor auth support
5. **Easy Authorization**: Seamless integration with `[Authorize]` attributes

## When to Use Identity vs JWT

### Use Identity Framework when:
- Building traditional web applications
- Need persistent user management
- Want built-in security features
- Claims change frequently
- Single application domain

### Use JWT when:
- Building APIs for mobile/SPA
- Need stateless authentication
- Microservices architecture
- Cross-domain authentication
- Claims are relatively static

This demo shows how Identity Framework provides a comprehensive authentication solution with automatic claims management, making it ideal for web applications where you need robust user management.
