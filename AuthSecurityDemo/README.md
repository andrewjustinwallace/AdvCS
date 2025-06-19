# ASP.NET Core Authentication & Security Demo

This project demonstrates JWT authentication, claims-based authorization, and security features (XSS/CSRF prevention) in ASP.NET Core 8 **without Entity Framework**. It uses Dapper for database access and SQL Server for data storage.

## 🚀 Features

### Authentication & Authorization
- **JWT Token Authentication** - JSON Web Token-based authentication
- **Claims-based Authorization** - Custom claims and policies
- **Role-based Access Control** - Admin, Manager, User roles
- **Custom User Store** - Using Dapper instead of Entity Framework
- **Password Hashing** - BCrypt for secure password storage

### Security Features
- **XSS Prevention** - Reflected, DOM-based, and Stored XSS protection
- **CSRF Protection** - Anti-forgery tokens on all forms
- **Content Security Policy** - HTTP headers for additional security
- **Input Validation** - Model validation and HTML encoding
- **Secure Cookies** - HttpOnly, Secure, SameSite attributes

## 🛠️ Setup Instructions

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB works fine)
- Visual Studio 2022 or VS Code

### 1. Clone and Build
```bash
cd D:\Dev\repos\AdvCS\AuthSecurityDemo
dotnet restore
dotnet build
```

### 2. Database Setup
The application automatically creates the database and tables on first run. The connection string in `appsettings.json` uses LocalDB:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AuthSecurityDemo;Trusted_Connection=true;MultipleActiveResultSets=true"
}
```

### 3. Run the Application
```bash
dotnet run
```

The application will be available at `https://localhost:5001` or `http://localhost:5000`.

## 📚 Learning Examples

### 1. JWT Authentication Flow

**Login Process:**
```csharp
// User submits credentials
var result = await _authService.LoginAsync(email, password);

// Service validates credentials and creates JWT
if (valid) {
    var token = GenerateJwtToken(user); // Contains claims
    return token;
}
```

**Token Structure:**
```
Header: { "alg": "HS256", "typ": "JWT" }
Payload: { 
  "sub": "123", 
  "email": "user@example.com",
  "role": "Admin",
  "age": "25",
  "exp": 1640995200 
}
Signature: HMACSHA256(header + payload, secret)
```

### 2. Claims-Based Authorization

**Controller Protection:**
```csharp
[Authorize(Roles = "Admin")]                    // Role-based
[Authorize(Policy = "MinimumAge18")]           // Policy-based
[Authorize(Policy = "ManagerOrAdmin")]         // Multiple roles
```

**Policy Configuration:**
```csharp
services.AddAuthorization(options => {
    options.AddPolicy("MinimumAge18", policy => 
        policy.RequireClaim("age", "18", "19", "20", ...));
    
    options.AddPolicy("ManagerOrAdmin", policy => 
        policy.RequireRole("Manager", "Admin"));
});
```

### 3. XSS Prevention Examples

**Reflected XSS Prevention:**
```csharp
// ✅ SAFE: Always HTML encode user input
ViewBag.SafeOutput = System.Web.HttpUtility.HtmlEncode(userInput);

// ❌ DANGEROUS: Never output raw user input
// ViewBag.DangerousOutput = userInput;
```

**Stored XSS Prevention:**
```csharp
// In UserService.AddCommentAsync()
var sanitizedContent = System.Web.HttpUtility.HtmlEncode(content);
await _connection.ExecuteAsync(sql, new { Content = sanitizedContent });
```

**DOM XSS Prevention:**
```javascript
// ✅ SAFE: Use textContent
element.textContent = userInput;

// ❌ DANGEROUS: Using innerHTML with user input
// element.innerHTML = userInput;
```

### 4. CSRF Protection

**Form Protection:**
```html
<form asp-action="Login" method="post">
    @Html.AntiForgeryToken()  <!-- Adds CSRF token -->
    <!-- form fields -->
</form>
```

**Controller Validation:**
```csharp
[HttpPost]
[ValidateAntiForgeryToken]  // Validates CSRF token
public async Task<IActionResult> Login(LoginModel model)
{
    // Action logic
}
```

## 🧪 Testing the Features

### 1. User Registration & Roles
1. Navigate to `/Auth/Register`
2. Create users with different ages (test age-based authorization)
3. Check the database to see how roles are assigned

### 2. Claims Demonstration
1. Login and go to `/Auth/Profile` to see your JWT claims
2. Try accessing `/Home/AdminPanel` (Admin only)
3. Try accessing `/Home/RestrictedContent` (18+ only)

### 3. API Testing
Use Postman or curl to test the API endpoints:

```bash
# Login and get JWT token
curl -X POST https://localhost:5001/api/api/login \
  -H "Content-Type: application/json" \
  -d '{"email":"user@example.com","password":"password123"}'

# Use token to access protected endpoint
curl -X GET https://localhost:5001/api/api/profile \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

### 4. Security Testing

**XSS Testing:**
1. Go to `/Home/XssDemo`
2. Try entering: `<script>alert('XSS')</script>`
3. Observe how it's safely encoded

**CSRF Testing:**
1. Open browser dev tools
2. Try submitting forms without the anti-forgery token
3. Should receive validation errors

## 📖 Key Learning Points

### Claims vs Roles
- **Roles** are special types of claims (`ClaimTypes.Role`)
- **Claims** can contain any user attribute (age, department, permissions)
- Claims provide more granular authorization than traditional roles

### JWT Security
- Tokens are **stateless** - no server-side storage needed
- Tokens are **signed** to prevent tampering
- Tokens should be stored securely (HttpOnly cookies recommended)
- Always validate tokens on protected endpoints

### XSS Prevention Layers
1. **Input Validation** - Validate at entry point
2. **Output Encoding** - HTML encode when displaying
3. **Content Security Policy** - Browser-level protection
4. **Secure Headers** - X-XSS-Protection, X-Content-Type-Options

### CSRF Protection Strategy
1. **Anti-forgery Tokens** - Unique tokens per form
2. **SameSite Cookies** - Prevent cross-site cookie sending
3. **Origin Validation** - Check request origin headers

## 🗂️ Project Structure

```
AuthSecurityDemo/
├── Controllers/
│   ├── AuthController.cs      # Login, Register, Profile
│   ├── HomeController.cs      # Main pages, XSS demos
│   └── ApiController.cs       # REST API endpoints
├── Models/
│   └── Models.cs              # User, Claims, API models
├── Services/
│   ├── IServices.cs           # Service interfaces
│   ├── AuthService.cs         # Authentication logic
│   └── UserService.cs         # User management, CRUD
├── Views/
│   ├── Home/                  # Home, Admin, Manager pages
│   ├── Auth/                  # Login, Register, Profile
│   └── Shared/                # Layout, validation scripts
└── Program.cs                 # Startup configuration
```

## 🔧 Configuration Details

### JWT Configuration (appsettings.json)
```json
{
  "Jwt": {
    "Key": "ThisIsAVeryLongSecretKeyThatShouldBeAtLeast256BitsLong!",
    "Issuer": "AuthSecurityDemo",
    "Audience": "AuthSecurityDemo-Users",
    "ExpiryInHours": 24
  }
}
```

### Database Schema
```sql
-- Users table
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Email NVARCHAR(256) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    FirstName NVARCHAR(100),
    LastName NVARCHAR(100),
    Age INT,
    CreatedAt DATETIME2 DEFAULT GETDATE()
);

-- Roles table
CREATE TABLE Roles (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(256) NOT NULL UNIQUE
);

-- User-Role junction table
CREATE TABLE UserRoles (
    UserId INT,
    RoleId INT,
    PRIMARY KEY (UserId, RoleId),
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (RoleId) REFERENCES Roles(Id)
);

-- Comments table (for XSS demonstration)
CREATE TABLE Comments (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);
```

## 🔍 Security Headers Applied

The application sets these security headers automatically:

```csharp
// In Program.cs middleware
context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
context.Response.Headers.Append("X-Frame-Options", "DENY");
context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
context.Response.Headers.Append("Content-Security-Policy", 
    "default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline'");
```

## 🚨 Common Security Mistakes Avoided

1. **Storing passwords in plain text** ❌
   - Solution: BCrypt hashing ✅

2. **Using innerHTML with user input** ❌
   - Solution: textContent or HTML encoding ✅

3. **Missing CSRF protection** ❌
   - Solution: Anti-forgery tokens ✅

4. **Weak JWT secrets** ❌
   - Solution: 256-bit cryptographically secure key ✅

5. **Exposing sensitive data in tokens** ❌
   - Solution: Only include necessary claims ✅

6. **No input validation** ❌
   - Solution: Model validation + HTML encoding ✅

## 📝 Additional Notes

### Production Considerations
- Use HTTPS everywhere (set `RequireHttpsMetadata = true`)
- Store JWT keys in secure configuration (Azure Key Vault, etc.)
- Implement rate limiting for authentication endpoints
- Add logging and monitoring for security events
- Consider implementing refresh tokens for long-lived sessions

### Testing Different Scenarios
1. **Create Admin User**: Manually insert into UserRoles table
2. **Test Age Restrictions**: Register users with different ages
3. **API Testing**: Use different tools (Postman, curl, Swagger)
4. **Security Testing**: Try XSS payloads and CSRF attacks

This demo provides a comprehensive foundation for understanding modern web authentication and security in ASP.NET Core without the complexity of Entity Framework!
