# Use the official .NET 8.0 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["src/BackendAPI/BackendAPI.csproj", "src/BackendAPI/"]
COPY ["src/BackendAPI.Application/BackendAPI.Application.csproj", "src/BackendAPI.Application/"]
COPY ["src/BackendAPI.Domain/BackendAPI.Domain.csproj", "src/BackendAPI.Domain/"]
COPY ["src/BackendAPI.Infrastructure/BackendAPI.Infrastructure.csproj", "src/BackendAPI.Infrastructure/"]

RUN dotnet restore "src/BackendAPI/BackendAPI.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/src/BackendAPI"
RUN dotnet build "BackendAPI.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "BackendAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the official .NET 8.0 runtime image for running
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create logs directory
RUN mkdir -p /app/logs

# Expose port 80
EXPOSE 80

ENTRYPOINT ["dotnet", "BackendAPI.dll"]
