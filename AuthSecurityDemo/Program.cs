using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AuthSecurityDemo.Services;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System.Data;
using Dapper;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews(options =>
{
    // Add global anti-forgery filter for all POST actions
    options.Filters.Add(new Microsoft.AspNetCore.Mvc.AutoValidateAntiforgeryTokenAttribute());
});

// Configure Database Connection (with fallback options)
var connectionString = builder.Configuration.GetConnectionString("SqlServerLocal") ;
builder.Services.AddScoped<IDbConnection>(provider =>
{
    if (connectionString.Contains("Data Source=") && connectionString.Contains(".db"))
    {
        return new SqliteConnection(connectionString);
    }
    else
    {
        return new SqlConnection(connectionString);
    }
});

// Register custom services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();

// Configure JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured");
var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Set to true in production
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["AuthToken"];
            return Task.CompletedTask;
        }
    };
});

// Add Authorization with policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("ManagerOrAdmin", policy => policy.RequireRole("Manager", "Admin"));
    options.AddPolicy("MinimumAge18", policy => policy.RequireClaim("age", "18", "19", "20", "21", "22", "23", "24", "25"));
});

// Add Antiforgery services for CSRF protection
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
    options.Cookie.Name = "__RequestVerificationToken";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Use Secure in production
    options.Cookie.SameSite = SameSiteMode.Strict;
});

var app = builder.Build();

// Configure pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Security headers to prevent XSS
app.Use(async (context, next) =>
{
    // Prevent XSS attacks
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
    
    // Content Security Policy to prevent XSS
    //context.Response.Headers.Append("Content-Security-Policy",
    //    "default-src 'self'; script-src 'self' 'unsafe-inline' https://cdn.jsdelivr.net; style-src 'self' 'unsafe-inline' https://cdn.jsdelivr.net; img-src 'self' data:;");

    await next();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Initialize database tables
await InitializeDatabase(app, connectionString);

app.Run();

async Task InitializeDatabase(WebApplication app, string connectionString)
{
    using var scope = app.Services.CreateScope();
    var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
    
    string createTablesScript;
    bool isSqlite = connectionString.Contains("Data Source=") && connectionString.Contains(".db");
    
    if (isSqlite)
    {
        Console.WriteLine("📄 Initializing SQLite database...");
        // SQLite syntax
        createTablesScript = @"
            CREATE TABLE IF NOT EXISTS Users (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Email TEXT NOT NULL UNIQUE,
                PasswordHash TEXT NOT NULL,
                FirstName TEXT,
                LastName TEXT,
                Age INTEGER,
                CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
            );

            CREATE TABLE IF NOT EXISTS Roles (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL UNIQUE
            );

            CREATE TABLE IF NOT EXISTS UserRoles (
                UserId INTEGER,
                RoleId INTEGER,
                PRIMARY KEY (UserId, RoleId),
                FOREIGN KEY (UserId) REFERENCES Users(Id),
                FOREIGN KEY (RoleId) REFERENCES Roles(Id)
            );

            CREATE TABLE IF NOT EXISTS Comments (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                UserId INTEGER NOT NULL,
                Content TEXT NOT NULL,
                CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                FOREIGN KEY (UserId) REFERENCES Users(Id)
            );

            INSERT OR IGNORE INTO Roles (Name) VALUES ('Admin');
            INSERT OR IGNORE INTO Roles (Name) VALUES ('Manager');
            INSERT OR IGNORE INTO Roles (Name) VALUES ('User');
        ";
    }
    else
    {
        Console.WriteLine("📄 Initializing SQL Server database...");
        // SQL Server syntax
        createTablesScript = @"
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
            CREATE TABLE Users (
                Id INT PRIMARY KEY IDENTITY(1,1),
                Email NVARCHAR(256) NOT NULL UNIQUE,
                PasswordHash NVARCHAR(MAX) NOT NULL,
                FirstName NVARCHAR(100),
                LastName NVARCHAR(100),
                Age INT,
                CreatedAt DATETIME2 DEFAULT GETDATE()
            );

            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Roles' AND xtype='U')
            CREATE TABLE Roles (
                Id INT PRIMARY KEY IDENTITY(1,1),
                Name NVARCHAR(256) NOT NULL UNIQUE
            );

            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='UserRoles' AND xtype='U')
            CREATE TABLE UserRoles (
                UserId INT,
                RoleId INT,
                PRIMARY KEY (UserId, RoleId),
                FOREIGN KEY (UserId) REFERENCES Users(Id),
                FOREIGN KEY (RoleId) REFERENCES Roles(Id)
            );

            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Comments' AND xtype='U')
            CREATE TABLE Comments (
                Id INT PRIMARY KEY IDENTITY(1,1),
                UserId INT NOT NULL,
                Content NVARCHAR(MAX) NOT NULL,
                CreatedAt DATETIME2 DEFAULT GETDATE(),
                FOREIGN KEY (UserId) REFERENCES Users(Id)
            );

            IF NOT EXISTS (SELECT * FROM Roles WHERE Name = 'Admin')
                INSERT INTO Roles (Name) VALUES ('Admin');
            
            IF NOT EXISTS (SELECT * FROM Roles WHERE Name = 'Manager')
                INSERT INTO Roles (Name) VALUES ('Manager');
                
            IF NOT EXISTS (SELECT * FROM Roles WHERE Name = 'User')
                INSERT INTO Roles (Name) VALUES ('User');
        ";
    }

    try
    {
        await connection.ExecuteAsync(createTablesScript);
        Console.WriteLine($"✅ Database initialized successfully ({(isSqlite ? "SQLite" : "SQL Server")})");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Database initialization failed: {ex.Message}");
        throw;
    }
}
