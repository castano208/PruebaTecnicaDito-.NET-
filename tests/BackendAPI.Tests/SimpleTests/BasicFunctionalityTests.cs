using BackendAPI.Domain.DTOs;
using BackendAPI.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace BackendAPI.Tests.SimpleTests;

public class BasicFunctionalityTests
{
    [Fact]
    public void UsuarioDto_ShouldHaveRequiredProperties()
    {
        // Arrange
        var usuario = new UsuarioDto
        {
            Id = 1,
            Nombre = "Juan",
            Apellido = "Pérez",
            Email = "juan@test.com",
            Telefono = "123456789",
            FechaNacimiento = DateTime.Now.AddYears(-25)
        };

        // Assert
        usuario.Should().NotBeNull();
        usuario.Id.Should().Be(1);
        usuario.Nombre.Should().Be("Juan");
        usuario.Apellido.Should().Be("Pérez");
        usuario.Email.Should().Be("juan@test.com");
    }

    [Fact]
    public void ProductoDto_ShouldHaveRequiredProperties()
    {
        // Arrange
        var producto = new ProductoDto
        {
            Id = 1,
            Nombre = "Laptop Dell",
            Descripcion = "Laptop de alta gama",
            Precio = 1500.00m,
            Stock = 10,
            Categoria = "Electrónicos",
            Codigo = "LAP001"
        };

        // Assert
        producto.Should().NotBeNull();
        producto.Id.Should().Be(1);
        producto.Nombre.Should().Be("Laptop Dell");
        producto.Precio.Should().Be(1500.00m);
        producto.Stock.Should().Be(10);
    }

    [Fact]
    public void PedidoDto_ShouldHaveRequiredProperties()
    {
        // Arrange
        var pedido = new PedidoDto
        {
            Id = 1,
            NumeroPedido = "PED001",
            Total = 2000.00m,
            Estado = "Pendiente",
            UsuarioId = 1,
            FechaPedido = DateTime.Now
        };

        // Assert
        pedido.Should().NotBeNull();
        pedido.Id.Should().Be(1);
        pedido.NumeroPedido.Should().Be("PED001");
        pedido.Total.Should().Be(2000.00m);
        pedido.Estado.Should().Be("Pendiente");
    }

    [Fact]
    public void LoginDto_ShouldHaveRequiredProperties()
    {
        // Arrange
        var login = new LoginDto
        {
            Email = "test@example.com",
            Password = "password123"
        };

        // Assert
        login.Should().NotBeNull();
        login.Email.Should().Be("test@example.com");
        login.Password.Should().Be("password123");
    }

    [Fact]
    public void RegisterDto_ShouldHaveRequiredProperties()
    {
        // Arrange
        var register = new RegisterDto
        {
            Nombre = "Test",
            Apellido = "User",
            Email = "test@example.com",
            Password = "password123",
            Telefono = "123456789",
            FechaNacimiento = DateTime.Now.AddYears(-25)
        };

        // Assert
        register.Should().NotBeNull();
        register.Nombre.Should().Be("Test");
        register.Email.Should().Be("test@example.com");
        register.Password.Should().Be("password123");
    }

    [Fact]
    public void AuthResponseDto_ShouldHaveRequiredProperties()
    {
        // Arrange
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

        // Assert
        authResponse.Should().NotBeNull();
        authResponse.Token.Should().Be("jwt-token");
        authResponse.RefreshToken.Should().Be("refresh-token");
        authResponse.Usuario.Should().NotBeNull();
    }

    [Fact]
    public void PaginationDto_ShouldHaveRequiredProperties()
    {
        // Arrange
        var pagination = new PaginationDto
        {
            Page = 1,
            PageSize = 10,
            Search = "test",
            SortBy = "nombre",
            SortDescending = false
        };

        // Assert
        pagination.Should().NotBeNull();
        pagination.Page.Should().Be(1);
        pagination.PageSize.Should().Be(10);
        pagination.Search.Should().Be("test");
        pagination.SortBy.Should().Be("nombre");
        pagination.SortDescending.Should().BeFalse();
    }

    [Fact]
    public void PagedResultDto_ShouldHaveRequiredProperties()
    {
        // Arrange
        var pagedResult = new PagedResultDto<UsuarioDto>
        {
            Data = new List<UsuarioDto>
            {
                new() { Id = 1, Nombre = "Juan", Apellido = "Pérez", Email = "juan@test.com" },
                new() { Id = 2, Nombre = "María", Apellido = "García", Email = "maria@test.com" }
            },
            TotalCount = 2,
            Page = 1,
            PageSize = 10
        };

        // Assert
        pagedResult.Should().NotBeNull();
        pagedResult.Data.Should().HaveCount(2);
        pagedResult.TotalCount.Should().Be(2);
        pagedResult.Page.Should().Be(1);
        pagedResult.PageSize.Should().Be(10);
        pagedResult.HasNextPage.Should().BeFalse();
        pagedResult.HasPreviousPage.Should().BeFalse();
    }

    [Fact]
    public void Usuario_Entity_ShouldHaveRequiredProperties()
    {
        // Arrange
        var usuario = new Usuario
        {
            Id = 1,
            Nombre = "Juan",
            Apellido = "Pérez",
            Email = "juan@test.com",
            Telefono = "123456789",
            FechaNacimiento = DateTime.Now.AddYears(-25),
            PasswordHash = "hashedpassword",
            FechaCreacion = DateTime.UtcNow,
            Activo = true
        };

        // Assert
        usuario.Should().NotBeNull();
        usuario.Id.Should().Be(1);
        usuario.Nombre.Should().Be("Juan");
        usuario.Email.Should().Be("juan@test.com");
        usuario.Activo.Should().BeTrue();
    }

    [Fact]
    public void Producto_Entity_ShouldHaveRequiredProperties()
    {
        // Arrange
        var producto = new Producto
        {
            Id = 1,
            Nombre = "Laptop Dell",
            Descripcion = "Laptop de alta gama",
            Precio = 1500.00m,
            Stock = 10,
            Categoria = "Electrónicos",
            Codigo = "LAP001",
            FechaCreacion = DateTime.UtcNow,
            Activo = true
        };

        // Assert
        producto.Should().NotBeNull();
        producto.Id.Should().Be(1);
        producto.Nombre.Should().Be("Laptop Dell");
        producto.Precio.Should().Be(1500.00m);
        producto.Stock.Should().Be(10);
        producto.Activo.Should().BeTrue();
    }

    [Fact]
    public void Pedido_Entity_ShouldHaveRequiredProperties()
    {
        // Arrange
        var pedido = new Pedido
        {
            Id = 1,
            NumeroPedido = "PED001",
            Total = 2000.00m,
            Estado = "Pendiente",
            UsuarioId = 1,
            FechaPedido = DateTime.Now,
            FechaCreacion = DateTime.UtcNow,
            Activo = true
        };

        // Assert
        pedido.Should().NotBeNull();
        pedido.Id.Should().Be(1);
        pedido.NumeroPedido.Should().Be("PED001");
        pedido.Total.Should().Be(2000.00m);
        pedido.Estado.Should().Be("Pendiente");
        pedido.Activo.Should().BeTrue();
    }
}
