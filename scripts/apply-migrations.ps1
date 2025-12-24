param(
    [string]$Project = "src/MilkProductsCatalog/MilkProductsCatalog.csproj",
    [string]$ConnectionString = $env:ConnectionStrings__SqlServer
)

if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    Write-Error "dotnet SDK is required."
    exit 1
}

if (-not (Get-Command dotnet-ef -ErrorAction SilentlyContinue)) {
    dotnet tool update -g dotnet-ef | Out-Null
}

$projectDir = Split-Path $Project -Parent
$migrationsPath = Join-Path $projectDir "Migrations"
if (-not (Test-Path $migrationsPath)) {
    Write-Host "No migrations found for $Project. Skipping apply-migrations."
    exit 0
}

$argsList = @("ef", "database", "update", "--project", $Project)
if ($ConnectionString) {
    $argsList += @("--connection", $ConnectionString)
}

Write-Host "Running: dotnet $($argsList -join ' ')"
dotnet @argsList
