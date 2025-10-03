using BackendAPI.Application.Features.Usuarios.Commands.CreateUsuario;
using BackendAPI.Application.Features.Usuarios.Commands.UpdateUsuario;
using BackendAPI.Application.Features.Usuarios.Commands.DeleteUsuario;
using BackendAPI.Application.Features.Usuarios.Queries.GetUsuarioById;
using BackendAPI.Application.Features.Usuarios.Queries.GetUsuarios;
using BackendAPI.Application.Common.Exceptions;
using BackendAPI.Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsuariosController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsuariosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todos los usuarios activos con paginación
    /// </summary>
    /// <param name="pagination">Parámetros de paginación</param>
    /// <returns>Lista paginada de usuarios</returns>
    [HttpGet]
    public async Task<ActionResult<PagedResultDto<UsuarioDto>>> GetUsuarios([FromQuery] PaginationDto pagination)
    {
        var usuarios = await _mediator.Send(new GetUsuariosQuery { Pagination = pagination });
        return Ok(usuarios);
    }

    /// <summary>
    /// Obtiene un usuario por ID
    /// </summary>
    /// <param name="id">ID del usuario</param>
    /// <returns>Usuario encontrado</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<UsuarioDto>> GetUsuario(int id)
    {
        try
        {
            var usuario = await _mediator.Send(new GetUsuarioByIdQuery { Id = id });
            return Ok(usuario);
        }
        catch (NotFoundException)
        {
            return NotFound(new { message = "Usuario no encontrado" });
        }
    }

    /// <summary>
    /// Crea un nuevo usuario
    /// </summary>
    /// <param name="command">Datos del usuario a crear</param>
    /// <returns>Usuario creado</returns>
    [HttpPost]
    public async Task<ActionResult<UsuarioDto>> CreateUsuario(CreateUsuarioCommand command)
    {
        try
        {
            var usuario = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message, errors = ex.Errors });
        }
    }

    /// <summary>
    /// Actualiza un usuario existente
    /// </summary>
    /// <param name="id">ID del usuario a actualizar</param>
    /// <param name="command">Datos actualizados del usuario</param>
    /// <returns>Usuario actualizado</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<UsuarioDto>> UpdateUsuario(int id, UpdateUsuarioCommand command)
    {
        try
        {
            command.Id = id;
            var usuario = await _mediator.Send(command);
            return Ok(usuario);
        }
        catch (NotFoundException)
        {
            return NotFound(new { message = "Usuario no encontrado" });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message, errors = ex.Errors });
        }
    }

    /// <summary>
    /// Elimina un usuario
    /// </summary>
    /// <param name="id">ID del usuario a eliminar</param>
    /// <returns>Resultado de la operación</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUsuario(int id)
    {
        try
        {
            await _mediator.Send(new DeleteUsuarioCommand { Id = id });
            return NoContent();
        }
        catch (NotFoundException)
        {
            return NotFound(new { message = "Usuario no encontrado" });
        }
    }
}
