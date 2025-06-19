using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();

// Configure Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "External";
})
.AddCookie("Cookies", options =>
{
    options.LoginPath = "/Auth/Login";
    options.LogoutPath = "/Auth/Logout";
    options.ExpireTimeSpan = TimeSpan.FromHours(24);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
})
.AddCookie("External")
.AddGoogle("Google", options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"] ?? "";
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ?? "";
    options.SignInScheme = "External";
    
    // Request additional scopes for more user info
    options.Scope.Add("profile");
    options.Scope.Add("email");
    
    // Map additional claims
    options.ClaimActions.MapJsonKey("picture", "picture");
    options.ClaimActions.MapJsonKey("locale", "locale");
    options.ClaimActions.MapJsonKey("email_verified", "email_verified");
})
.AddMicrosoftAccount("Microsoft", options =>
{
    options.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"] ?? "";
    options.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"] ?? "";
    options.SignInScheme = "External";
})
.AddFacebook("Facebook", options =>
{
    options.AppId = builder.Configuration["Authentication:Facebook:AppId"] ?? "";
    options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"] ?? "";
    options.SignInScheme = "External";
    
    // Request additional permissions
    options.Scope.Add("email");
    options.Fields.Add("picture");
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

Console.WriteLine("🚀 OAuth Demo started");
Console.WriteLine("📝 Configure OAuth providers in appsettings.json");

app.Run();