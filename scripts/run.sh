#!/bin/bash

# Bash script to run the Backend API

echo "Starting Backend API..."

# Check if .NET 8 is installed
if ! command -v dotnet &> /dev/null; then
    echo "Error: .NET 8 SDK is not installed or not in PATH"
    echo "Please install .NET 8 SDK from: https://dotnet.microsoft.com/download/dotnet/8.0"
    exit 1
fi

DOTNET_VERSION=$(dotnet --version)
echo "Using .NET version: $DOTNET_VERSION"

# Restore packages
echo "Restoring packages..."
dotnet restore

if [ $? -ne 0 ]; then
    echo "Error: Failed to restore packages"
    exit 1
fi

# Build the solution
echo "Building solution..."
dotnet build

if [ $? -ne 0 ]; then
    echo "Error: Build failed"
    exit 1
fi

# Run tests
echo "Running tests..."
dotnet test

if [ $? -ne 0 ]; then
    echo "Warning: Some tests failed"
fi

# Run the API
echo "Starting API..."
echo "API will be available at: https://localhost:5001"
echo "Swagger UI will be available at: https://localhost:5001/swagger"
echo "Press Ctrl+C to stop the application"

dotnet run --project src/BackendAPI
