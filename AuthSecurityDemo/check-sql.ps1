# SQL Server Troubleshooting Script

Write-Host "🔍 SQL Server Connection Troubleshooting" -ForegroundColor Cyan
Write-Host "=========================================" -ForegroundColor Cyan
Write-Host ""

# Check for SQL Server LocalDB
Write-Host "1. Checking for SQL Server LocalDB..." -ForegroundColor Yellow
try {
    $localDbInfo = sqllocaldb info 2>$null
    if ($localDbInfo) {
        Write-Host "✅ LocalDB instances found:" -ForegroundColor Green
        sqllocaldb info
        Write-Host ""
        
        # Try to start the default instance
        Write-Host "Starting LocalDB instance..." -ForegroundColor Yellow
        sqllocaldb start mssqllocaldb 2>$null
        
        # Check if it's running
        $instances = sqllocaldb info
        if ($instances -contains "mssqllocaldb") {
            Write-Host "✅ LocalDB is available" -ForegroundColor Green
        }
    } else {
        Write-Host "❌ LocalDB not found" -ForegroundColor Red
    }
} catch {
    Write-Host "❌ LocalDB command not available" -ForegroundColor Red
}

Write-Host ""

# Check for SQL Server services
Write-Host "2. Checking for SQL Server services..." -ForegroundColor Yellow
$sqlServices = Get-Service | Where-Object {$_.Name -like "*SQL*"} | Select-Object Name, Status, DisplayName

if ($sqlServices) {
    Write-Host "✅ SQL Server services found:" -ForegroundColor Green
    $sqlServices | Format-Table -AutoSize
} else {
    Write-Host "❌ No SQL Server services found" -ForegroundColor Red
}

Write-Host ""

# Test connection strings
Write-Host "3. Testing connection strings..." -ForegroundColor Yellow

$connectionStrings = @{
    "LocalDB" = "Server=(localdb)\mssqllocaldb;Database=TestConnection;Integrated Security=true"
    "SQL Express" = "Server=.\SQLEXPRESS;Database=TestConnection;Integrated Security=true"
    "Local SQL Server" = "Server=localhost;Database=TestConnection;Integrated Security=true"
    "Local SQL Server (Named Pipes)" = "Server=np:localhost;Database=TestConnection;Integrated Security=true"
}

foreach ($name in $connectionStrings.Keys) {
    $connStr = $connectionStrings[$name]
    Write-Host "Testing $name..." -ForegroundColor Gray
    
    try {
        $connection = New-Object System.Data.SqlClient.SqlConnection($connStr)
        $connection.Open()
        $connection.Close()
        Write-Host "✅ $name: Connection successful" -ForegroundColor Green
    } catch {
        Write-Host "❌ $name: $($_.Exception.Message)" -ForegroundColor Red
    }
}

Write-Host ""

# Recommendations
Write-Host "🔧 Recommendations:" -ForegroundColor Cyan
Write-Host ""

if (-not $localDbInfo) {
    Write-Host "📦 Install SQL Server LocalDB:" -ForegroundColor Yellow
    Write-Host "   Option 1: Download SQL Server Express with LocalDB" -ForegroundColor Gray
    Write-Host "   https://www.microsoft.com/en-us/sql-server/sql-server-downloads" -ForegroundColor Gray
    Write-Host ""
    Write-Host "   Option 2: Install via Visual Studio Installer" -ForegroundColor Gray
    Write-Host "   - Open Visual Studio Installer" -ForegroundColor Gray
    Write-Host "   - Modify installation → Individual Components" -ForegroundColor Gray
    Write-Host "   - Check 'SQL Server Express LocalDB'" -ForegroundColor Gray
    Write-Host ""
}

Write-Host "🔄 Alternative: Use SQLite (No SQL Server Required)" -ForegroundColor Yellow
Write-Host "   The demo app will automatically fall back to SQLite if SQL Server is unavailable." -ForegroundColor Gray
Write-Host "   SQLite provides the same functionality for this demo." -ForegroundColor Gray
Write-Host ""

Write-Host "▶️  To run the demo with automatic database selection:" -ForegroundColor Green
Write-Host "   cd D:\Dev\repos\AdvCS\AuthSecurityDemo" -ForegroundColor Gray
Write-Host "   dotnet run" -ForegroundColor Gray
Write-Host ""

Write-Host "The app will try SQL Server first, then fall back to SQLite automatically." -ForegroundColor Cyan
