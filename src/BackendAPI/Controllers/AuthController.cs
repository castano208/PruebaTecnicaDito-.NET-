using BackendAPI.Application.Common.Interfaces;
using BackendAPI.Application.Common.Exceptions;
using BackendAPI.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Iniciar sesión
    /// </summary>
    /// <param name="loginDto">Credenciales de acceso</param>
    /// <returns>Token de autenticación</returns>
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
    {
        try
        {
            var result = await _authService.LoginAsync(loginDto);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Registrar nuevo usuario
    /// </summary>
    /// <param name="registerDto">Datos del usuario</param>
    /// <returns>Token de autenticación</returns>
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto registerDto)
    {
        try
        {
            var result = await _authService.RegisterAsync(registerDto);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Renovar token de acceso
    /// </summary>
    /// <param name="refreshTokenDto">Token de actualización</param>
    /// <returns>Nuevo token de autenticación</returns>
    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponseDto>> RefreshToken(RefreshTokenDto refreshTokenDto)
    {
        try
        {
            var result = await _authService.RefreshTokenAsync(refreshTokenDto.RefreshToken);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Cerrar sesión
    /// </summary>
    /// <param name="refreshTokenDto">Token de actualización</param>
    /// <returns>Resultado de la operación</returns>
    [HttpPost("logout")]
    public async Task<ActionResult> Logout(RefreshTokenDto refreshTokenDto)
    {
        try
        {
            var result = await _authService.RevokeTokenAsync(refreshTokenDto.RefreshToken);
            if (result)
            {
                return Ok(new { message = "Sesión cerrada exitosamente" });
            }
            return BadRequest(new { message = "Token inválido" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Revocar token de actualización
    /// </summary>
    /// <param name="refreshTokenDto">Token de actualización</param>
    /// <returns>Resultado de la operación</returns>
    [HttpPost("revoke")]
    public async Task<ActionResult> RevokeToken(RefreshTokenDto refreshTokenDto)
    {
        try
        {
            var result = await _authService.RevokeTokenAsync(refreshTokenDto.RefreshToken);
            if (result)
            {
                return Ok(new { message = "Token revocado exitosamente" });
            }
            return BadRequest(new { message = "Token inválido" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Revocar token JWT actual
    /// </summary>
    /// <returns>Resultado de la operación</returns>
    [HttpPost("revoke-jwt")]
    public async Task<ActionResult> RevokeJwtToken()
    {
        try
        {
            // Extraer el token del header Authorization
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            if (authHeader == null || !authHeader.StartsWith("Bearer "))
            {
                return BadRequest(new { message = "Token no encontrado" });
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();
            var result = await _authService.RevokeJwtTokenAsync(token);
            
            if (result)
            {
                return Ok(new { message = "Token JWT revocado exitosamente" });
            }
            return BadRequest(new { message = "Token inválido" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
