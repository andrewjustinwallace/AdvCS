# SQL Server SSL Certificate Troubleshooting Script

Write-Host "🔍 SQL Server SSL Certificate Troubleshooting" -ForegroundColor Cyan
Write-Host "===============================================" -ForegroundColor Cyan
Write-Host ""

# Test different connection string configurations
Write-Host "🔧 Testing SQL Server connection with different SSL configurations..." -ForegroundColor Yellow
Write-Host ""

$connectionTests = @{
    "LocalDB with TrustServerCertificate" = "Server=(localdb)\mssqllocaldb;Database=TestConnection;Integrated Security=true;TrustServerCertificate=true;"
    "LocalDB with Encrypt=false" = "Server=(localdb)\mssqllocaldb;Database=TestConnection;Integrated Security=true;Encrypt=false;"
    "LocalDB without SSL options" = "Server=(localdb)\mssqllocaldb;Database=TestConnection;Integrated Security=true;"
    "SQL Express with TrustServerCertificate" = "Server=.\SQLEXPRESS;Database=TestConnection;Integrated Security=true;TrustServerCertificate=true;"
    "SQL Express with Encrypt=false" = "Server=.\SQLEXPRESS;Database=TestConnection;Integrated Security=true;Encrypt=false;"
}

$successfulConnections = @()

foreach ($name in $connectionTests.Keys) {
    $connStr = $connectionTests[$name]
    Write-Host "Testing: $name" -ForegroundColor Gray
    
    try {
        # Load SQL Client assembly
        Add-Type -AssemblyName "System.Data.SqlClient"
        
        $connection = New-Object System.Data.SqlClient.SqlConnection($connStr)
        $connection.Open()
        $connection.Close()
        Write-Host "✅ SUCCESS: $name" -ForegroundColor Green
        $successfulConnections += $name
    } catch {
        $errorMsg = $_.Exception.Message
        if ($errorMsg -like "*certificate*" -or $errorMsg -like "*SSL*" -or $errorMsg -like "*TLS*") {
            Write-Host "❌ SSL/Certificate Error: $name" -ForegroundColor Red
            Write-Host "   Error: $errorMsg" -ForegroundColor DarkRed
        } elseif ($errorMsg -like "*login*" -or $errorMsg -like "*authentication*") {
            Write-Host "🔐 Authentication Error: $name" -ForegroundColor Yellow
            Write-Host "   Error: $errorMsg" -ForegroundColor DarkYellow
        } else {
            Write-Host "❌ Connection Error: $name" -ForegroundColor Red
            Write-Host "   Error: $errorMsg" -ForegroundColor DarkRed
        }
    }
    Write-Host ""
}

# Show results
Write-Host "📊 Results Summary:" -ForegroundColor Cyan
Write-Host "==================" -ForegroundColor Cyan

if ($successfulConnections.Count -gt 0) {
    Write-Host "✅ Working connection methods:" -ForegroundColor Green
    foreach ($conn in $successfulConnections) {
        Write-Host "   • $conn" -ForegroundColor White
    }
} else {
    Write-Host "❌ No SQL Server connections working" -ForegroundColor Red
}

Write-Host ""

# Recommendations
Write-Host "🛠️  Solutions for SSL Certificate Issues:" -ForegroundColor Yellow
Write-Host "=========================================" -ForegroundColor Yellow
Write-Host ""

Write-Host "1. 🔧 Use TrustServerCertificate=true (Recommended for development)" -ForegroundColor Cyan
Write-Host "   Connection string: Server=(localdb)\mssqllocaldb;Database=YourDB;Trusted_Connection=true;TrustServerCertificate=true;" -ForegroundColor Gray
Write-Host ""

Write-Host "2. 🔧 Disable encryption with Encrypt=false" -ForegroundColor Cyan
Write-Host "   Connection string: Server=(localdb)\mssqllocaldb;Database=YourDB;Trusted_Connection=true;Encrypt=false;" -ForegroundColor Gray
Write-Host ""

Write-Host "3. 🔧 Install/Update SQL Server LocalDB to latest version" -ForegroundColor Cyan
Write-Host "   Download from: https://www.microsoft.com/en-us/sql-server/sql-server-downloads" -ForegroundColor Gray
Write-Host ""

Write-Host "4. 🔄 Use SQLite instead (No SSL issues)" -ForegroundColor Cyan
Write-Host "   The demo app automatically falls back to SQLite" -ForegroundColor Gray
Write-Host "   Connection string: Data Source=AuthSecurityDemo.db" -ForegroundColor Gray
Write-Host ""

# Check LocalDB version
Write-Host "🔍 LocalDB Version Check:" -ForegroundColor Yellow
try {
    $localDbVersion = sqllocaldb info | Out-String
    if ($localDbVersion) {
        Write-Host "LocalDB instances:" -ForegroundColor Gray
        sqllocaldb info
        Write-Host ""
        
        # Try to get version info
        try {
            $versionInfo = sqllocaldb versions
            Write-Host "Available LocalDB versions:" -ForegroundColor Gray
            $versionInfo
        } catch {
            Write-Host "Could not get version information" -ForegroundColor Yellow
        }
    }
} catch {
    Write-Host "❌ LocalDB not available" -ForegroundColor Red
}

Write-Host ""
Write-Host "🚀 Quick Fix - Run the Demo App:" -ForegroundColor Green
Write-Host "================================" -ForegroundColor Green
Write-Host "The demo app has been updated with automatic SSL handling." -ForegroundColor White
Write-Host "It will try multiple connection methods and fall back to SQLite if needed." -ForegroundColor White
Write-Host ""
Write-Host "Run this command:" -ForegroundColor Yellow
Write-Host "  cd D:\Dev\repos\AdvCS\AuthSecurityDemo" -ForegroundColor Gray
Write-Host "  dotnet run" -ForegroundColor Gray
Write-Host ""
Write-Host "The app will automatically:" -ForegroundColor Cyan
Write-Host "  ✅ Try SQL Server with TrustServerCertificate=true" -ForegroundColor White
Write-Host "  ✅ Try SQL Server with Encrypt=false" -ForegroundColor White
Write-Host "  ✅ Fall back to SQLite (always works)" -ForegroundColor White
Write-Host "  ✅ Show you which database it's using" -ForegroundColor White
