using BackendAPI.Application.Common.Interfaces;
using BackendAPI.Application.Common.Services;
using BackendAPI.Domain.DTOs;
using BackendAPI.Domain.Entities;
using BackendAPI.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using AutoMapper;
using BackendAPI.Application.Common.Exceptions;

namespace BackendAPI.Tests.Common.Services;

public class AuthServiceTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _mapperMock = new Mock<IMapper>();
        _configurationMock = new Mock<IConfiguration>();
        
        // Setup configuration
        _configurationMock.Setup(x => x["Jwt:Key"]).Returns("test-secret-key-that-is-long-enough-for-jwt-token-generation");
        _configurationMock.Setup(x => x["Jwt:Issuer"]).Returns("TestIssuer");
        _configurationMock.Setup(x => x["Jwt:Audience"]).Returns("TestAudience");

        _authService = new AuthService(_usuarioRepositoryMock.Object, _mapperMock.Object, _configurationMock.Object);
    }

    [Fact]
    public void AuthService_ShouldBeCreated()
    {
        // Assert
        _authService.Should().NotBeNull();
    }

    [Fact]
    public void GenerateJwtToken_WithValidUser_ShouldReturnToken()
    {
        // Arrange
        var usuario = new UsuarioDto
        {
            Id = 1,
            Nombre = "Test",
            Apellido = "User",
            Email = "test@example.com"
        };

        // Act
        var result = _authService.GenerateJwtToken(usuario);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain(".");
    }

    [Fact]
    public void GenerateRefreshToken_ShouldReturnBase64String()
    {
        // Act
        var result = _authService.GenerateRefreshToken();

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Length.Should().BeGreaterThan(20);
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ShouldReturnAuthResponse()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "test@example.com",
            Password = "password123"
        };

        var usuario = new Usuario
        {
            Id = 1,
            Email = loginDto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(loginDto.Password),
            Nombre = "Test",
            Apellido = "User",
            Activo = true,
            UsuarioRoles = new List<UsuarioRol>()
        };

        var usuarioDto = new UsuarioDto
        {
            Id = 1,
            Email = loginDto.Email,
            Nombre = "Test",
            Apellido = "User"
        };

        _usuarioRepositoryMock.Setup(x => x.GetByEmailAsync(loginDto.Email))
            .ReturnsAsync(usuario);

        _mapperMock.Setup(x => x.Map<UsuarioDto>(usuario))
            .Returns(usuarioDto);

        _usuarioRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Usuario>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _authService.LoginAsync(loginDto);

        // Assert
        result.Should().NotBeNull();
        result.Token.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBeNullOrEmpty();
        result.Usuario.Should().NotBeNull();
        result.Usuario.Email.Should().Be(loginDto.Email);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidEmail_ShouldThrowValidationException()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "nonexistent@example.com",
            Password = "password123"
        };

        _usuarioRepositoryMock.Setup(x => x.GetByEmailAsync(loginDto.Email))
            .ReturnsAsync((Usuario?)null);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.LoginAsync(loginDto));
    }

    [Fact]
    public async Task LoginAsync_WithInvalidPassword_ShouldThrowValidationException()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "test@example.com",
            Password = "wrongpassword"
        };

        var usuario = new Usuario
        {
            Id = 1,
            Email = loginDto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("correctpassword"),
            Activo = true
        };

        _usuarioRepositoryMock.Setup(x => x.GetByEmailAsync(loginDto.Email))
            .ReturnsAsync(usuario);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.LoginAsync(loginDto));
    }

    [Fact]
    public async Task RegisterAsync_WithValidData_ShouldReturnAuthResponse()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Nombre = "Test",
            Apellido = "User",
            Email = "test@example.com",
            Password = "password123",
            Telefono = "123456789",
            FechaNacimiento = DateTime.Now.AddYears(-25)
        };

        var usuario = new Usuario
        {
            Id = 1,
            Nombre = registerDto.Nombre,
            Apellido = registerDto.Apellido,
            Email = registerDto.Email,
            Telefono = registerDto.Telefono,
            FechaNacimiento = registerDto.FechaNacimiento,
            Activo = true,
            UsuarioRoles = new List<UsuarioRol>()
        };

        var usuarioDto = new UsuarioDto
        {
            Id = 1,
            Nombre = registerDto.Nombre,
            Apellido = registerDto.Apellido,
            Email = registerDto.Email
        };

        _usuarioRepositoryMock.Setup(x => x.EmailExistsAsync(registerDto.Email, null))
            .ReturnsAsync(false);

        _usuarioRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Usuario>()))
            .ReturnsAsync(usuario);

        _mapperMock.Setup(x => x.Map<Usuario>(registerDto))
            .Returns(usuario);

        _mapperMock.Setup(x => x.Map<UsuarioDto>(usuario))
            .Returns(usuarioDto);

        _usuarioRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Usuario>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _authService.RegisterAsync(registerDto);

        // Assert
        result.Should().NotBeNull();
        result.Token.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBeNullOrEmpty();
        result.Usuario.Should().NotBeNull();
        result.Usuario.Email.Should().Be(registerDto.Email);
    }

    [Fact]
    public async Task RegisterAsync_WithExistingEmail_ShouldThrowValidationException()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Nombre = "Test",
            Apellido = "User",
            Email = "existing@example.com",
            Password = "password123",
            Telefono = "123456789",
            FechaNacimiento = DateTime.Now.AddYears(-25)
        };

        _usuarioRepositoryMock.Setup(x => x.EmailExistsAsync(registerDto.Email, null))
            .ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _authService.RegisterAsync(registerDto));
    }

    [Fact]
    public async Task RefreshTokenAsync_WithValidToken_ShouldReturnNewTokens()
    {
        // Arrange
        var refreshToken = "valid-refresh-token";
        var usuario = new Usuario
        {
            Id = 1,
            Email = "test@example.com",
            RefreshToken = refreshToken,
            RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1),
            Activo = true,
            UsuarioRoles = new List<UsuarioRol>()
        };

        _usuarioRepositoryMock.Setup(x => x.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Usuario, bool>>>()))
            .ReturnsAsync(usuario);

        _usuarioRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Usuario>()))
            .Returns(Task.CompletedTask);

        _mapperMock.Setup(x => x.Map<UsuarioDto>(usuario))
            .Returns(new UsuarioDto { Id = usuario.Id, Email = usuario.Email, Nombre = usuario.Nombre, Apellido = usuario.Apellido });

        // Act
        var result = await _authService.RefreshTokenAsync(refreshToken);

        // Assert
        result.Should().NotBeNull();
        result.Token.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBe(refreshToken); // Should be a new token
    }

    [Fact]
    public async Task RefreshTokenAsync_WithExpiredToken_ShouldThrowValidationException()
    {
        // Arrange
        var refreshToken = "expired-refresh-token";
        var usuario = new Usuario
        {
            Id = 1,
            Email = "test@example.com",
            RefreshToken = refreshToken,
            RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(-1), // Expired
            Activo = true
        };

        _usuarioRepositoryMock.Setup(x => x.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Usuario, bool>>>()))
            .ReturnsAsync(usuario);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.RefreshTokenAsync(refreshToken));
    }

    [Fact]
    public async Task RevokeTokenAsync_WithValidToken_ShouldReturnTrue()
    {
        // Arrange
        var refreshToken = "valid-refresh-token";
        var usuario = new Usuario
        {
            Id = 1,
            Email = "test@example.com",
            RefreshToken = refreshToken,
            Activo = true
        };

        _usuarioRepositoryMock.Setup(x => x.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Usuario, bool>>>()))
            .ReturnsAsync(usuario);

        _usuarioRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Usuario>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _authService.RevokeTokenAsync(refreshToken);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task RevokeTokenAsync_WithInvalidToken_ShouldReturnFalse()
    {
        // Arrange
        var refreshToken = "invalid-refresh-token";

        _usuarioRepositoryMock.Setup(x => x.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Usuario, bool>>>()))
            .ReturnsAsync((Usuario?)null);

        // Act
        var result = await _authService.RevokeTokenAsync(refreshToken);

        // Assert
        result.Should().BeFalse();
    }
}