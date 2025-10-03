using BackendAPI.Controllers;
using BackendAPI.Application.Features.Usuarios.Commands.CreateUsuario;
using BackendAPI.Application.Features.Usuarios.Queries.GetUsuarios;
using BackendAPI.Application.Features.Usuarios.Queries.GetUsuarioById;
using BackendAPI.Domain.DTOs;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BackendAPI.Tests.Controllers;

public class UsuariosControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly UsuariosController _controller;

    public UsuariosControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new UsuariosController(_mediatorMock.Object);
    }

    [Fact]
    public void UsuariosController_ShouldBeCreated()
    {
        // Assert
        _controller.Should().NotBeNull();
    }

    [Fact]
    public async Task GetUsuarios_WithValidPagination_ShouldReturnOkResult()
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

        var expectedResult = new PagedResultDto<UsuarioDto>
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

        _mediatorMock.Setup(x => x.Send(It.IsAny<GetUsuariosQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetUsuarios(pagination);

        // Assert
        result.Should().BeOfType<ActionResult<PagedResultDto<UsuarioDto>>>();
        var okResult = result.Result as OkObjectResult;
        okResult?.Value.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetUsuario_WithValidId_ShouldReturnOkResult()
    {
        // Arrange
        var userId = 1;
        var expectedUsuario = new UsuarioDto
        {
            Id = 1,
            Nombre = "Juan",
            Apellido = "Pérez",
            Email = "juan@test.com"
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<GetUsuarioByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedUsuario);

        // Act
        var result = await _controller.GetUsuario(userId);

        // Assert
        result.Should().BeOfType<ActionResult<UsuarioDto>>();
        var okResult = result.Result as OkObjectResult;
        okResult?.Value.Should().BeEquivalentTo(expectedUsuario);
    }

    [Fact]
    public async Task CreateUsuario_WithValidData_ShouldReturnCreatedResult()
    {
        // Arrange
        var command = new CreateUsuarioCommand
        {
            Nombre = "Juan",
            Apellido = "Pérez",
            Email = "juan@test.com",
            Telefono = "123456789",
            FechaNacimiento = DateTime.Now.AddYears(-25)
        };

        var expectedUsuario = new UsuarioDto
        {
            Id = 1,
            Nombre = command.Nombre,
            Apellido = command.Apellido,
            Email = command.Email
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateUsuarioCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedUsuario);

        // Act
        var result = await _controller.CreateUsuario(command);

        // Assert
        result.Should().BeOfType<ActionResult<UsuarioDto>>();
        var createdResult = result.Result as CreatedAtActionResult;
        createdResult?.Value.Should().BeEquivalentTo(expectedUsuario);
    }

    [Fact]
    public async Task GetUsuarios_WithEmptySearch_ShouldReturnAllUsers()
    {
        // Arrange
        var pagination = new PaginationDto
        {
            Page = 1,
            PageSize = 10
        };

        var expectedResult = new PagedResultDto<UsuarioDto>
        {
            Data = new List<UsuarioDto>(),
            TotalCount = 0,
            Page = 1,
            PageSize = 10
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<GetUsuariosQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetUsuarios(pagination);

        // Assert
        result.Should().BeOfType<ActionResult<PagedResultDto<UsuarioDto>>>();
    }
}
