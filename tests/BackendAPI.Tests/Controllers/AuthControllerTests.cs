using BackendAPI.Application.Common.Interfaces;
using BackendAPI.Controllers;
using BackendAPI.Domain.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BackendAPI.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _authServiceMock = new Mock<IAuthService>();
        _controller = new AuthController(_authServiceMock.Object);
    }

    [Fact]
    public void AuthController_ShouldBeCreated()
    {
        // Assert
        _controller.Should().NotBeNull();
    }

    [Fact]
    public async Task Login_WithValidCredentials_ShouldReturnActionResult()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "test@example.com",
            Password = "password123"
        };

        var authResponse = new AuthResponseDto
        {
            Token = "jwt-token",
            RefreshToken = "refresh-token",
            Usuario = new UsuarioDto
            {
                Id = 1,
                Nombre = "Test",
                Apellido = "User",
                Email = "test@example.com"
            }
        };

        _authServiceMock.Setup(x => x.LoginAsync(loginDto))
            .ReturnsAsync(authResponse);

        // Act
        var result = await _controller.Login(loginDto);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ActionResult<AuthResponseDto>>();
    }
}