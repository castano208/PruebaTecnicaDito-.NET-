using BackendAPI.Application.Features.Usuarios.Commands.CreateUsuario;
using FluentAssertions;
using Xunit;

namespace BackendAPI.Tests.Features.Usuarios.Commands.CreateUsuario;

public class CreateUsuarioCommandValidatorTests
{
    private readonly CreateUsuarioCommandValidator _validator;

    public CreateUsuarioCommandValidatorTests()
    {
        _validator = new CreateUsuarioCommandValidator();
    }

    [Fact]
    public void CreateUsuarioCommandValidator_ShouldBeCreated()
    {
        // Assert
        _validator.Should().NotBeNull();
    }

    [Fact]
    public void Validate_WithValidData_ShouldPass()
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

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithEmptyNombre_ShouldFail()
    {
        // Arrange
        var command = new CreateUsuarioCommand
        {
            Nombre = "",
            Apellido = "Pérez",
            Email = "juan@example.com",
            Telefono = "123456789",
            FechaNacimiento = DateTime.Now.AddYears(-25)
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Nombre");
    }

    [Fact]
    public void Validate_WithEmptyApellido_ShouldFail()
    {
        // Arrange
        var command = new CreateUsuarioCommand
        {
            Nombre = "Juan",
            Apellido = "",
            Email = "juan@example.com",
            Telefono = "123456789",
            FechaNacimiento = DateTime.Now.AddYears(-25)
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Apellido");
    }

    [Fact]
    public void Validate_WithInvalidEmail_ShouldFail()
    {
        // Arrange
        var command = new CreateUsuarioCommand
        {
            Nombre = "Juan",
            Apellido = "Pérez",
            Email = "invalid-email",
            Telefono = "123456789",
            FechaNacimiento = DateTime.Now.AddYears(-25)
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Fact]
    public void Validate_WithEmptyEmail_ShouldFail()
    {
        // Arrange
        var command = new CreateUsuarioCommand
        {
            Nombre = "Juan",
            Apellido = "Pérez",
            Email = "",
            Telefono = "123456789",
            FechaNacimiento = DateTime.Now.AddYears(-25)
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Fact]
    public void Validate_WithFutureFechaNacimiento_ShouldFail()
    {
        // Arrange
        var command = new CreateUsuarioCommand
        {
            Nombre = "Juan",
            Apellido = "Pérez",
            Email = "juan@example.com",
            Telefono = "123456789",
            FechaNacimiento = DateTime.Now.AddYears(1)
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "FechaNacimiento");
    }

    [Fact]
    public void Validate_WithLongTelefono_ShouldFail()
    {
        // Arrange
        var command = new CreateUsuarioCommand
        {
            Nombre = "Juan",
            Apellido = "Pérez",
            Email = "juan@example.com",
            Telefono = "123456789012345678901", // Too long (21 characters, max is 20)
            FechaNacimiento = DateTime.Now.AddYears(-25)
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Telefono");
    }

    [Fact]
    public void Validate_WithMinimalValidData_ShouldPass()
    {
        // Arrange
        var command = new CreateUsuarioCommand
        {
            Nombre = "Juan",
            Apellido = "Pérez",
            Email = "juan@example.com",
            Telefono = "1234567890",
            FechaNacimiento = DateTime.Now.AddYears(-25)
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
