using BackendAPI.Application.Features.Pedidos.Commands.CreatePedido;
using BackendAPI.Application.Features.Pedidos.Queries.GetPedidoById;
using BackendAPI.Application.Features.Pedidos.Queries.GetPedidos;
using BackendAPI.Application.Common.Exceptions;
using BackendAPI.Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Controllers;

/// <summary>
/// Controlador para manejo de pedidos
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Constructor del controlador de pedidos
    /// </summary>
    /// <param name="mediator">Mediador para manejo de comandos y consultas</param>
    public PedidosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todos los pedidos
    /// </summary>
    /// <returns>Lista de pedidos</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PedidoDto>>> GetPedidos()
    {
        var pedidos = await _mediator.Send(new GetPedidosQuery());
        return Ok(pedidos);
    }

    /// <summary>
    /// Obtiene un pedido por ID
    /// </summary>
    /// <param name="id">ID del pedido</param>
    /// <returns>Pedido encontrado</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<PedidoDto>> GetPedido(int id)
    {
        try
        {
            var pedido = await _mediator.Send(new GetPedidoByIdQuery { Id = id });
            return Ok(pedido);
        }
        catch (NotFoundException)
        {
            return NotFound(new { message = "Pedido no encontrado" });
        }
    }

    /// <summary>
    /// Crea un nuevo pedido
    /// </summary>
    /// <param name="command">Datos del pedido a crear</param>
    /// <returns>Pedido creado</returns>
    [HttpPost]
    public async Task<ActionResult<PedidoDto>> CreatePedido(CreatePedidoCommand command)
    {
        try
        {
            var pedido = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetPedido), new { id = pedido.Id }, pedido);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message, errors = ex.Errors });
        }
    }
}
