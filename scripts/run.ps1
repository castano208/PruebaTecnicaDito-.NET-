# PowerShell script to run the Backend API

Write-Host "Starting Backend API..." -ForegroundColor Green

# Check if .NET 8 is installed
try {
    $dotnetVersion = dotnet --version
    Write-Host "Using .NET version: $dotnetVersion" -ForegroundColor Yellow
} catch {
    Write-Host "Error: .NET 8 SDK is not installed or not in PATH" -ForegroundColor Red
    Write-Host "Please install .NET 8 SDK from: https://dotnet.microsoft.com/download/dotnet/8.0" -ForegroundColor Yellow
    exit 1
}

# Restore packages
Write-Host "Restoring packages..." -ForegroundColor Yellow
dotnet restore

if ($LASTEXITCODE -ne 0) {
    Write-Host "Error: Failed to restore packages" -ForegroundColor Red
    exit 1
}

# Build the solution
Write-Host "Building solution..." -ForegroundColor Yellow
dotnet build

if ($LASTEXITCODE -ne 0) {
    Write-Host "Error: Build failed" -ForegroundColor Red
    exit 1
}

# Run tests
Write-Host "Running tests..." -ForegroundColor Yellow
dotnet test

if ($LASTEXITCODE -ne 0) {
    Write-Host "Warning: Some tests failed" -ForegroundColor Yellow
}

# Run the API
Write-Host "Starting API..." -ForegroundColor Green
Write-Host "API will be available at: https://localhost:5001" -ForegroundColor Cyan
Write-Host "Swagger UI will be available at: https://localhost:5001/swagger" -ForegroundColor Cyan
Write-Host "Press Ctrl+C to stop the application" -ForegroundColor Yellow

dotnet run --project src/BackendAPI
