# PowerShell script to install Playwright for this solution
# Run from repository root: .\scripts\install-playwright.ps1

# 1) Add the Playwright .NET packages to the test project
$project = "Revenue.Tests.VehicleRego.BDD.csproj"
Write-Host "Adding NuGet packages to $project..."
dotnet add $project package Microsoft.Playwright
# Optional: NUnit integration helpers for Playwright tests
dotnet add $project package Playwright.NUnit --prerelease

# 2) Create a local dotnet tool manifest (if it doesn't exist) and install the Playwright CLI as a local tool
Write-Host "Creating dotnet tool manifest and installing Microsoft.Playwright.CLI (local tool)..."
dotnet new tool-manifest --force
dotnet tool install Microsoft.Playwright.CLI

# 3) Restore packages
Write-Host "Restoring packages..."
dotnet restore

# 4) Run the Playwright CLI to download browser binaries
Write-Host "Running 'playwright install' to download browser binaries..."
dotnet tool run playwright install

Write-Host "Playwright installation steps finished. If any command fails, run the commands manually to see detailed errors."