# Script para probar la configuracion de CI/CD localmente
# Simula los comandos que ejecuta GitHub Actions

Write-Host "Probando configuracion de CI/CD localmente..." -ForegroundColor Green

# Variables
$SOLUTION_FILE = "BackendAPI.sln"
$TEST_PROJECT = "tests/BackendAPI.Tests/BackendAPI.Tests.csproj"
$API_PROJECT = "src/BackendAPI/BackendAPI.csproj"

Write-Host "Variables de configuracion:" -ForegroundColor Yellow
Write-Host "  SOLUTION_FILE: $SOLUTION_FILE"
Write-Host "  TEST_PROJECT: $TEST_PROJECT"
Write-Host "  API_PROJECT: $API_PROJECT"

# Paso 1: Restaurar dependencias
Write-Host "Paso 1: Restaurando dependencias..." -ForegroundColor Cyan
dotnet restore $SOLUTION_FILE
if ($LASTEXITCODE -ne 0) {
    Write-Host "Error en restore" -ForegroundColor Red
    exit 1
}

# Paso 2: Compilar solucion
Write-Host "Paso 2: Compilando solucion..." -ForegroundColor Cyan
dotnet build $SOLUTION_FILE --no-restore --configuration Release
if ($LASTEXITCODE -ne 0) {
    Write-Host "Error en build" -ForegroundColor Red
    exit 1
}

# Paso 3: Limpiar directorio de resultados
Write-Host "Paso 3: Limpiando directorio de resultados..." -ForegroundColor Cyan
if (Test-Path "./TestResults") {
    Remove-Item -Recurse -Force "./TestResults"
}
New-Item -ItemType Directory -Path "./TestResults" -Force

# Paso 4: Ejecutar tests
Write-Host "Paso 4: Ejecutando tests..." -ForegroundColor Cyan
dotnet test $TEST_PROJECT --no-build --configuration Release --verbosity normal --collect:"XPlat Code Coverage" --logger trx --results-directory ./TestResults

# Paso 5: Verificar archivos generados
Write-Host "Paso 5: Verificando archivos generados..." -ForegroundColor Cyan
Write-Host "Archivos en TestResults:"
Get-ChildItem -Path "./TestResults" -Recurse | ForEach-Object {
    Write-Host "  $($_.FullName)" -ForegroundColor White
}

# Verificar archivos especificos
$trxFiles = Get-ChildItem -Path "./TestResults" -Filter "*.trx" -Recurse
$xmlFiles = Get-ChildItem -Path "./TestResults" -Filter "*.xml" -Recurse
$coverageFiles = Get-ChildItem -Path "./TestResults" -Filter "coverage.cobertura.xml" -Recurse

Write-Host "Resumen de archivos generados:" -ForegroundColor Yellow
Write-Host "  Archivos TRX: $($trxFiles.Count)"
Write-Host "  Archivos XML: $($xmlFiles.Count)"
Write-Host "  Archivos de cobertura: $($coverageFiles.Count)"

if ($trxFiles.Count -eq 0) {
    Write-Host "No se encontraron archivos TRX" -ForegroundColor Yellow
}

if ($xmlFiles.Count -eq 0) {
    Write-Host "No se encontraron archivos XML" -ForegroundColor Yellow
}

if ($coverageFiles.Count -eq 0) {
    Write-Host "No se encontraron archivos de cobertura" -ForegroundColor Yellow
}

Write-Host "Prueba de CI/CD completada" -ForegroundColor Green
Write-Host "Si todos los archivos se generaron correctamente, el pipeline deberia funcionar en GitHub Actions" -ForegroundColor Green