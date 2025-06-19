# PowerShell script to set up and run the Auth Security Demo

Write-Host "🚀 Setting up ASP.NET Core Auth Security Demo..." -ForegroundColor Green

# Navigate to project directory
$ProjectPath = "D:\Dev\repos\AdvCS\AuthSecurityDemo"

if (Test-Path $ProjectPath) {
    Set-Location $ProjectPath
    Write-Host "✅ Found project directory: $ProjectPath" -ForegroundColor Green
} else {
    Write-Host "❌ Project directory not found: $ProjectPath" -ForegroundColor Red
    exit 1
}

# Check if .NET 8 is installed
Write-Host "🔍 Checking .NET version..." -ForegroundColor Yellow
try {
    $dotnetVersion = dotnet --version
    if ($dotnetVersion -like "8.*") {
        Write-Host "✅ .NET 8 found: $dotnetVersion" -ForegroundColor Green
    } else {
        Write-Host "❌ .NET 8 required. Current version: $dotnetVersion" -ForegroundColor Red
        Write-Host "Please install .NET 8 SDK from https://dotnet.microsoft.com/download" -ForegroundColor Yellow
        exit 1
    }
} catch {
    Write-Host "❌ .NET not found. Please install .NET 8 SDK from https://dotnet.microsoft.com/download" -ForegroundColor Red
    exit 1
}

# Clean previous builds
Write-Host "🧹 Cleaning previous builds..." -ForegroundColor Yellow
dotnet clean

# Clear package cache if there are issues
Write-Host "📦 Clearing NuGet cache..." -ForegroundColor Yellow
dotnet nuget locals all --clear

# Restore packages
Write-Host "📦 Restoring NuGet packages..." -ForegroundColor Yellow
dotnet restore --verbosity minimal

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Packages restored successfully" -ForegroundColor Green
} else {
    Write-Host "❌ Failed to restore packages. Trying with more verbose output..." -ForegroundColor Red
    Write-Host "Running: dotnet restore --verbosity normal" -ForegroundColor Yellow
    dotnet restore --verbosity normal
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "❌ Package restore failed. Please check the error messages above." -ForegroundColor Red
        Write-Host "💡 Try running these commands manually:" -ForegroundColor Yellow
        Write-Host "   dotnet nuget locals all --clear" -ForegroundColor Gray
        Write-Host "   dotnet restore" -ForegroundColor Gray
        Write-Host "   dotnet build" -ForegroundColor Gray
        exit 1
    }
}

# Build project
Write-Host "🔨 Building project..." -ForegroundColor Yellow
dotnet build --verbosity minimal

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Build successful" -ForegroundColor Green
} else {
    Write-Host "❌ Build failed. Trying with more verbose output..." -ForegroundColor Red
    Write-Host "Running: dotnet build --verbosity normal" -ForegroundColor Yellow
    dotnet build --verbosity normal
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "❌ Build failed. Please check the error messages above." -ForegroundColor Red
        exit 1
    }
}

# Display startup information
Write-Host ""
Write-Host "🎉 Setup Complete!" -ForegroundColor Green
Write-Host ""
Write-Host "📚 What this demo includes:" -ForegroundColor Cyan
Write-Host "  • JWT Authentication (no Entity Framework)" -ForegroundColor White
Write-Host "  • Claims-based Authorization" -ForegroundColor White
Write-Host "  • Role-based Access Control" -ForegroundColor White
Write-Host "  • XSS Prevention (Reflected, DOM, Stored)" -ForegroundColor White
Write-Host "  • CSRF Protection with Anti-forgery Tokens" -ForegroundColor White
Write-Host "  • Dapper + SQL Server data access" -ForegroundColor White
Write-Host "  • BCrypt password hashing" -ForegroundColor White
Write-Host ""
Write-Host "🌐 To start the application:" -ForegroundColor Cyan
Write-Host "  dotnet run" -ForegroundColor Yellow
Write-Host ""
Write-Host "📍 Application URLs:" -ForegroundColor Cyan
Write-Host "  HTTPS: https://localhost:5001" -ForegroundColor Yellow
Write-Host "  HTTP:  http://localhost:5000" -ForegroundColor Yellow
Write-Host ""
Write-Host "🧪 Try these features:" -ForegroundColor Cyan
Write-Host "  1. Register a new user (try different ages for claims demo)" -ForegroundColor White
Write-Host "  2. Go to /Home/XssDemo to test XSS prevention" -ForegroundColor White
Write-Host "  3. Login and check /Auth/Profile to see JWT claims" -ForegroundColor White
Write-Host "  4. Test API endpoints with Postman:" -ForegroundColor White
Write-Host "     POST /api/api/login" -ForegroundColor Gray
Write-Host "     GET  /api/api/profile (requires auth)" -ForegroundColor Gray
Write-Host "     GET  /api/api/claims-demo (requires auth)" -ForegroundColor Gray
Write-Host ""
Write-Host "📖 For detailed documentation, see README.md" -ForegroundColor Cyan
Write-Host ""
Write-Host "🔧 If you encounter issues:" -ForegroundColor Yellow
Write-Host "  • Make sure SQL Server LocalDB is installed" -ForegroundColor Gray
Write-Host "  • Check that no other app is using ports 5000/5001" -ForegroundColor Gray
Write-Host "  • Review the connection string in appsettings.json" -ForegroundColor Gray
Write-Host ""

# Ask if user wants to run the application
$runApp = Read-Host "Would you like to start the application now? (y/n)"
if ($runApp -eq 'y' -or $runApp -eq 'Y') {
    Write-Host "🚀 Starting application..." -ForegroundColor Green
    Write-Host "Press Ctrl+C to stop the application" -ForegroundColor Yellow
    Write-Host ""
    dotnet run
} else {
    Write-Host "👍 To start later, run: dotnet run" -ForegroundColor Green
}
