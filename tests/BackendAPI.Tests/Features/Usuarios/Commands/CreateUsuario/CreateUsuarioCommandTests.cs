using BackendAPI.Application.Features.Usuarios.Commands.CreateUsuario;
using BackendAPI.Application.Common.Exceptions;
using BackendAPI.Domain.Entities;
using BackendAPI.Domain.Interfaces;
using BackendAPI.Domain.DTOs;
using FluentAssertions;
using Moq;
using Xunit;
using AutoMapper;

namespace BackendAPI.Tests.Features.Usuarios.Commands.CreateUsuario;

public class CreateUsuarioCommandTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateUsuarioCommandHandler _handler;

    public CreateUsuarioCommandTests()
    {
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new CreateUsuarioCommandHandler(_usuarioRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public void CreateUsuarioCommandHandler_ShouldBeCreated()
    {
        // Assert
        _handler.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_WhenEmailExists_ShouldThrowValidationException()
    {
        // Arrange
        var command = new CreateUsuarioCommand
        {
            Nombre = "Juan",
            Apellido = "Pérez",
            Email = "juan@example.com",
            Telefono = "123456789",
            FechaNacimiento = DateTime.Now.AddYears(-25)
        };

        _usuarioRepositoryMock.Setup(x => x.EmailExistsAsync(command.Email, null))
            .ReturnsAsync(true);

        // Act & Assert
        var cancellationToken = CancellationToken.None;
        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, cancellationToken));
    }

    [Fact]
    public async Task Handle_WhenEmailDoesNotExist_ShouldCreateUsuario()
    {
        // Arrange
        var command = new CreateUsuarioCommand
        {
            Nombre = "Juan",
            Apellido = "Pérez",
            Email = "juan@example.com",
            Telefono = "123456789",
            FechaNacimiento = DateTime.Now.AddYears(-25)
        };

        var createdUsuario = new Usuario
        {
            Id = 1,
            Nombre = command.Nombre,
            Apellido = command.Apellido,
            Email = command.Email,
            Telefono = command.Telefono,
            FechaNacimiento = command.FechaNacimiento,
            FechaCreacion = DateTime.UtcNow,
            Activo = true
        };

        var expectedDto = new UsuarioDto
        {
            Id = 1,
            Nombre = command.Nombre,
            Apellido = command.Apellido,
            Email = command.Email,
            Telefono = command.Telefono,
            FechaNacimiento = command.FechaNacimiento
        };

        _usuarioRepositoryMock.Setup(x => x.EmailExistsAsync(command.Email, null))
            .ReturnsAsync(false);

        _mapperMock.Setup(x => x.Map<Usuario>(command))
            .Returns(createdUsuario);

        _usuarioRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Usuario>()))
            .ReturnsAsync(createdUsuario);

        _mapperMock.Setup(x => x.Map<UsuarioDto>(createdUsuario))
            .Returns(expectedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Nombre.Should().Be(command.Nombre);
        result.Email.Should().Be(command.Email);
        
        _usuarioRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Usuario>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithMinimalData_ShouldCreateUsuario()
    {
        // Arrange
        var command = new CreateUsuarioCommand
        {
            Nombre = "Juan",
            Apellido = "Pérez",
            Email = "juan@example.com",
            FechaNacimiento = DateTime.Now.AddYears(-25)
        };

        var createdUsuario = new Usuario
        {
            Id = 1,
            Nombre = command.Nombre,
            Apellido = command.Apellido,
            Email = command.Email,
            FechaNacimiento = command.FechaNacimiento,
            FechaCreacion = DateTime.UtcNow,
            Activo = true
        };

        var expectedDto = new UsuarioDto
        {
            Id = 1,
            Nombre = command.Nombre,
            Apellido = command.Apellido,
            Email = command.Email,
            FechaNacimiento = command.FechaNacimiento
        };

        _usuarioRepositoryMock.Setup(x => x.EmailExistsAsync(command.Email, null))
            .ReturnsAsync(false);

        _mapperMock.Setup(x => x.Map<Usuario>(command))
            .Returns(createdUsuario);

        _usuarioRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Usuario>()))
            .ReturnsAsync(createdUsuario);

        _mapperMock.Setup(x => x.Map<UsuarioDto>(createdUsuario))
            .Returns(expectedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Nombre.Should().Be(command.Nombre);
        result.Email.Should().Be(command.Email);
    }
}