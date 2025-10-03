using BackendAPI.Domain.Entities;
using BackendAPI.Domain.Interfaces;
using BackendAPI.Infrastructure.Data;
using BackendAPI.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace BackendAPI.Tests.Infrastructure.Repositories;

public class UsuarioRepositoryTests
{
    private readonly DbContextOptions<ApplicationDbContext> _options;
    private readonly ApplicationDbContext _context;
    private readonly UsuarioRepository _repository;

    public UsuarioRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(_options);
        _repository = new UsuarioRepository(_context);
    }

    [Fact]
    public void UsuarioRepository_ShouldBeCreated()
    {
        // Assert
        _repository.Should().NotBeNull();
    }

    [Fact]
    public async Task AddAsync_WithValidUsuario_ShouldReturnUsuario()
    {
        // Arrange
        var usuario = new Usuario
        {
            Nombre = "Juan",
            Apellido = "Pérez",
            Email = "juan@test.com",
            Telefono = "123456789",
            FechaNacimiento = DateTime.Now.AddYears(-25),
            PasswordHash = "hashedpassword",
            FechaCreacion = DateTime.UtcNow,
            Activo = true
        };

        // Act
        var result = await _repository.AddAsync(usuario);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.Nombre.Should().Be(usuario.Nombre);
        result.Email.Should().Be(usuario.Email);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnUsuario()
    {
        // Arrange
        var usuario = new Usuario
        {
            Nombre = "Juan",
            Apellido = "Pérez",
            Email = "juan@test.com",
            Telefono = "123456789",
            FechaNacimiento = DateTime.Now.AddYears(-25),
            PasswordHash = "hashedpassword",
            FechaCreacion = DateTime.UtcNow,
            Activo = true
        };

        await _repository.AddAsync(usuario);

        // Act
        var result = await _repository.GetByIdAsync(usuario.Id);

        // Assert
        result.Should().NotBeNull();
        result?.Id.Should().Be(usuario.Id);
        result?.Nombre.Should().Be(usuario.Nombre);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByEmailAsync_WithValidEmail_ShouldReturnUsuario()
    {
        // Arrange
        var email = "juan@test.com";
        var usuario = new Usuario
        {
            Nombre = "Juan",
            Apellido = "Pérez",
            Email = email,
            Telefono = "123456789",
            FechaNacimiento = DateTime.Now.AddYears(-25),
            PasswordHash = "hashedpassword",
            FechaCreacion = DateTime.UtcNow,
            Activo = true
        };

        await _repository.AddAsync(usuario);

        // Act
        var result = await _repository.GetByEmailAsync(email);

        // Assert
        result.Should().NotBeNull();
        result?.Email.Should().Be(email);
    }

    [Fact]
    public async Task GetByEmailAsync_WithInvalidEmail_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetByEmailAsync("nonexistent@test.com");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task EmailExistsAsync_WithExistingEmail_ShouldReturnTrue()
    {
        // Arrange
        var email = "juan@test.com";
        var usuario = new Usuario
        {
            Nombre = "Juan",
            Apellido = "Pérez",
            Email = email,
            Telefono = "123456789",
            FechaNacimiento = DateTime.Now.AddYears(-25),
            PasswordHash = "hashedpassword",
            FechaCreacion = DateTime.UtcNow,
            Activo = true
        };

        await _repository.AddAsync(usuario);

        // Act
        var result = await _repository.EmailExistsAsync(email);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task EmailExistsAsync_WithNonExistingEmail_ShouldReturnFalse()
    {
        // Act
        var result = await _repository.EmailExistsAsync("nonexistent@test.com");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task EmailExistsAsync_WithExcludeId_ShouldReturnFalse()
    {
        // Arrange
        var email = "juan@test.com";
        var usuario = new Usuario
        {
            Nombre = "Juan",
            Apellido = "Pérez",
            Email = email,
            Telefono = "123456789",
            FechaNacimiento = DateTime.Now.AddYears(-25),
            PasswordHash = "hashedpassword",
            FechaCreacion = DateTime.UtcNow,
            Activo = true
        };

        await _repository.AddAsync(usuario);

        // Act
        var result = await _repository.EmailExistsAsync(email, usuario.Id);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task GetActiveUsersAsync_ShouldReturnOnlyActiveUsers()
    {
        // Arrange
        var activeUsuario = new Usuario
        {
            Nombre = "Juan",
            Apellido = "Pérez",
            Email = "juan@test.com",
            Telefono = "123456789",
            FechaNacimiento = DateTime.Now.AddYears(-25),
            PasswordHash = "hashedpassword",
            FechaCreacion = DateTime.UtcNow,
            Activo = true
        };

        var inactiveUsuario = new Usuario
        {
            Nombre = "María",
            Apellido = "García",
            Email = "maria@test.com",
            Telefono = "987654321",
            FechaNacimiento = DateTime.Now.AddYears(-30),
            PasswordHash = "hashedpassword",
            FechaCreacion = DateTime.UtcNow,
            Activo = false
        };

        await _repository.AddAsync(activeUsuario);
        await _repository.AddAsync(inactiveUsuario);

        // Act
        var result = await _repository.GetActiveUsersAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().Activo.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateAsync_WithValidUsuario_ShouldUpdateUsuario()
    {
        // Arrange
        var usuario = new Usuario
        {
            Nombre = "Juan",
            Apellido = "Pérez",
            Email = "juan@test.com",
            Telefono = "123456789",
            FechaNacimiento = DateTime.Now.AddYears(-25),
            PasswordHash = "hashedpassword",
            FechaCreacion = DateTime.UtcNow,
            Activo = true
        };

        await _repository.AddAsync(usuario);

        // Act
        usuario.Nombre = "Juan Carlos";
        usuario.Telefono = "987654321";
        await _repository.UpdateAsync(usuario);
        var result = await _repository.GetByIdAsync(usuario.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Nombre.Should().Be("Juan Carlos");
        result.Telefono.Should().Be("987654321");
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_ShouldDeleteUsuario()
    {
        // Arrange
        var usuario = new Usuario
        {
            Nombre = "Juan",
            Apellido = "Pérez",
            Email = "juan@test.com",
            Telefono = "123456789",
            FechaNacimiento = DateTime.Now.AddYears(-25),
            PasswordHash = "hashedpassword",
            FechaCreacion = DateTime.UtcNow,
            Activo = true
        };

        await _repository.AddAsync(usuario);

        // Act
        await _repository.DeleteAsync(usuario);

        // Assert
        var result = await _repository.GetByIdAsync(usuario.Id);
        result.Should().BeNull();
    }

    [Fact]
    public void GetActiveUsersQueryable_ShouldReturnQueryable()
    {
        // Act
        var result = _repository.GetActiveUsersQueryable();

        // Assert
        result.Should().NotBeNull();
    }
}
