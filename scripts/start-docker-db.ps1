param(
    [switch]$Detach
)

$arguments = @("-f", "docker/docker-compose.yml", "--env-file", ".env", "up")
if ($Detach) {
    $arguments += "-d"
}

Write-Host "Running: docker compose $($arguments -join ' ')"
docker compose @arguments
