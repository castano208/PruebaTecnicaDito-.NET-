using BackendAPI.Controllers;
using BackendAPI.Application.Features.Pedidos.Commands.CreatePedido;
using BackendAPI.Application.Features.Pedidos.Queries.GetPedidos;
using BackendAPI.Application.Features.Pedidos.Queries.GetPedidoById;
using BackendAPI.Domain.DTOs;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BackendAPI.Tests.Controllers;

public class PedidosControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly PedidosController _controller;

    public PedidosControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new PedidosController(_mediatorMock.Object);
    }

    [Fact]
    public void PedidosController_ShouldBeCreated()
    {
        // Assert
        _controller.Should().NotBeNull();
    }

    [Fact]
    public async Task GetPedidos_ShouldReturnOkResult()
    {
        // Arrange
        var expectedPedidos = new List<PedidoDto>
        {
            new() 
            { 
                Id = 1, 
                NumeroPedido = "PED001", 
                Total = 1500.00m, 
                Estado = "Pendiente",
                UsuarioId = 1
            },
            new() 
            { 
                Id = 2, 
                NumeroPedido = "PED002", 
                Total = 2000.00m, 
                Estado = "Procesando",
                UsuarioId = 2
            }
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<GetPedidosQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedPedidos);

        // Act
        var result = await _controller.GetPedidos();

        // Assert
        result.Should().BeOfType<ActionResult<IEnumerable<PedidoDto>>>();
        var okResult = result.Result as OkObjectResult;
        okResult?.Value.Should().BeEquivalentTo(expectedPedidos);
    }

    [Fact]
    public async Task GetPedido_WithValidId_ShouldReturnOkResult()
    {
        // Arrange
        var pedidoId = 1;
        var expectedPedido = new PedidoDto
        {
            Id = 1,
            NumeroPedido = "PED001",
            Total = 1500.00m,
            Estado = "Pendiente",
            UsuarioId = 1,
            FechaPedido = DateTime.Now
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<GetPedidoByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedPedido);

        // Act
        var result = await _controller.GetPedido(pedidoId);

        // Assert
        result.Should().BeOfType<ActionResult<PedidoDto>>();
        var okResult = result.Result as OkObjectResult;
        okResult?.Value.Should().BeEquivalentTo(expectedPedido);
    }

    [Fact]
    public async Task CreatePedido_WithValidData_ShouldReturnCreatedResult()
    {
        // Arrange
        var command = new CreatePedidoCommand
        {
            UsuarioId = 1,
            PedidoItems = new List<CreatePedidoItemDto>
            {
                new() { ProductoId = 1, Cantidad = 2 },
                new() { ProductoId = 2, Cantidad = 1 }
            },
            Comentarios = "Pedido urgente",
            DireccionEntrega = "Calle 123, Ciudad"
        };

        var expectedPedido = new PedidoDto
        {
            Id = 1,
            NumeroPedido = "PED001",
            Total = 2000.00m,
            Estado = "Pendiente",
            UsuarioId = 1,
            Comentarios = command.Comentarios,
            DireccionEntrega = command.DireccionEntrega
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<CreatePedidoCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedPedido);

        // Act
        var result = await _controller.CreatePedido(command);

        // Assert
        result.Should().BeOfType<ActionResult<PedidoDto>>();
        var createdResult = result.Result as CreatedAtActionResult;
        createdResult?.Value.Should().BeEquivalentTo(expectedPedido);
    }

    [Fact]
    public async Task GetPedidos_WithMultiplePedidos_ShouldReturnAllPedidos()
    {
        // Arrange
        var expectedPedidos = new List<PedidoDto>
        {
            new() { Id = 1, NumeroPedido = "PED001", Estado = "Pendiente" },
            new() { Id = 2, NumeroPedido = "PED003", Estado = "Pendiente" }
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<GetPedidosQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedPedidos);

        // Act
        var result = await _controller.GetPedidos();

        // Assert
        result.Should().BeOfType<ActionResult<IEnumerable<PedidoDto>>>();
        var okResult = result.Result as OkObjectResult;
        okResult?.Value.Should().BeEquivalentTo(expectedPedidos);
    }
}
