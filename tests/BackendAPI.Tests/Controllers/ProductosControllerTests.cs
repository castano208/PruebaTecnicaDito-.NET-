using BackendAPI.Controllers;
using BackendAPI.Application.Features.Productos.Commands.CreateProducto;
using BackendAPI.Application.Features.Productos.Queries.GetProductos;
using BackendAPI.Application.Features.Productos.Queries.GetProductoById;
using BackendAPI.Domain.DTOs;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BackendAPI.Tests.Controllers;

public class ProductosControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ProductosController _controller;

    public ProductosControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new ProductosController(_mediatorMock.Object);
    }

    [Fact]
    public void ProductosController_ShouldBeCreated()
    {
        // Assert
        _controller.Should().NotBeNull();
    }

    [Fact]
    public async Task GetProductos_ShouldReturnOkResult()
    {
        // Arrange
        var expectedProductos = new List<ProductoDto>
        {
            new() { Id = 1, Nombre = "Laptop Dell", Precio = 1500.00m, Stock = 10, Categoria = "Electrónicos" },
            new() { Id = 2, Nombre = "Laptop HP", Precio = 1200.00m, Stock = 5, Categoria = "Electrónicos" }
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<GetProductosQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedProductos);

        // Act
        var result = await _controller.GetProductos();

        // Assert
        result.Should().BeOfType<ActionResult<IEnumerable<ProductoDto>>>();
        var okResult = result.Result as OkObjectResult;
        okResult?.Value.Should().BeEquivalentTo(expectedProductos);
    }

    [Fact]
    public async Task GetProducto_WithValidId_ShouldReturnOkResult()
    {
        // Arrange
        var productoId = 1;
        var expectedProducto = new ProductoDto
        {
            Id = 1,
            Nombre = "Laptop Dell",
            Precio = 1500.00m,
            Stock = 10,
            Categoria = "Electrónicos"
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<GetProductoByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedProducto);

        // Act
        var result = await _controller.GetProducto(productoId);

        // Assert
        result.Should().BeOfType<ActionResult<ProductoDto>>();
        var okResult = result.Result as OkObjectResult;
        okResult?.Value.Should().BeEquivalentTo(expectedProducto);
    }

    [Fact]
    public async Task CreateProducto_WithValidData_ShouldReturnCreatedResult()
    {
        // Arrange
        var command = new CreateProductoCommand
        {
            Nombre = "Laptop Dell",
            Descripcion = "Laptop de alta gama",
            Precio = 1500.00m,
            Stock = 10,
            Categoria = "Electrónicos",
            Codigo = "LAP001"
        };

        var expectedProducto = new ProductoDto
        {
            Id = 1,
            Nombre = command.Nombre,
            Descripcion = command.Descripcion,
            Precio = command.Precio,
            Stock = command.Stock,
            Categoria = command.Categoria,
            Codigo = command.Codigo
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateProductoCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedProducto);

        // Act
        var result = await _controller.CreateProducto(command);

        // Assert
        result.Should().BeOfType<ActionResult<ProductoDto>>();
        var createdResult = result.Result as CreatedAtActionResult;
        createdResult?.Value.Should().BeEquivalentTo(expectedProducto);
    }

    [Fact]
    public async Task GetProductos_WithMultipleProductos_ShouldReturnAllProductos()
    {
        // Arrange
        var expectedProductos = new List<ProductoDto>
        {
            new() { Id = 1, Nombre = "Laptop Dell", Categoria = "Electrónicos" },
            new() { Id = 2, Nombre = "Mouse Logitech", Categoria = "Electrónicos" }
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<GetProductosQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedProductos);

        // Act
        var result = await _controller.GetProductos();

        // Assert
        result.Should().BeOfType<ActionResult<IEnumerable<ProductoDto>>>();
        var okResult = result.Result as OkObjectResult;
        okResult?.Value.Should().BeEquivalentTo(expectedProductos);
    }
}
