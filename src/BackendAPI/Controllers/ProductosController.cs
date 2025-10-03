using BackendAPI.Application.Features.Productos.Commands.CreateProducto;
using BackendAPI.Application.Features.Productos.Commands.UpdateProducto;
using BackendAPI.Application.Features.Productos.Commands.DeleteProducto;
using BackendAPI.Application.Features.Productos.Queries.GetProductoById;
using BackendAPI.Application.Features.Productos.Queries.GetProductos;
using BackendAPI.Application.Common.Exceptions;
using BackendAPI.Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Controllers;

/// <summary>
/// Controlador para manejo de productos
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Constructor del controlador de productos
    /// </summary>
    /// <param name="mediator">Mediador para manejo de comandos y consultas</param>
    public ProductosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todos los productos activos
    /// </summary>
    /// <returns>Lista de productos</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductoDto>>> GetProductos()
    {
        var productos = await _mediator.Send(new GetProductosQuery());
        return Ok(productos);
    }

    /// <summary>
    /// Obtiene un producto por ID
    /// </summary>
    /// <param name="id">ID del producto</param>
    /// <returns>Producto encontrado</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductoDto>> GetProducto(int id)
    {
        try
        {
            var producto = await _mediator.Send(new GetProductoByIdQuery { Id = id });
            return Ok(producto);
        }
        catch (NotFoundException)
        {
            return NotFound(new { message = "Producto no encontrado" });
        }
    }

    /// <summary>
    /// Crea un nuevo producto
    /// </summary>
    /// <param name="command">Datos del producto a crear</param>
    /// <returns>Producto creado</returns>
    [HttpPost]
    public async Task<ActionResult<ProductoDto>> CreateProducto(CreateProductoCommand command)
    {
        try
        {
            var producto = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetProducto), new { id = producto.Id }, producto);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message, errors = ex.Errors });
        }
    }

    /// <summary>
    /// Actualiza un producto existente
    /// </summary>
    /// <param name="id">ID del producto a actualizar</param>
    /// <param name="command">Datos actualizados del producto</param>
    /// <returns>Producto actualizado</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<ProductoDto>> UpdateProducto(int id, UpdateProductoCommand command)
    {
        try
        {
            command.Id = id;
            var producto = await _mediator.Send(command);
            return Ok(producto);
        }
        catch (NotFoundException)
        {
            return NotFound(new { message = "Producto no encontrado" });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message, errors = ex.Errors });
        }
    }

    /// <summary>
    /// Elimina un producto
    /// </summary>
    /// <param name="id">ID del producto a eliminar</param>
    /// <returns>Resultado de la operación</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProducto(int id)
    {
        try
        {
            await _mediator.Send(new DeleteProductoCommand { Id = id });
            return NoContent();
        }
        catch (NotFoundException)
        {
            return NotFound(new { message = "Producto no encontrado" });
        }
    }
}
