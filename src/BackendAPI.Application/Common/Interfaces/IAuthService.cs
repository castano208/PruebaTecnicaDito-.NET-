using BackendAPI.Domain.DTOs;

namespace BackendAPI.Application.Common.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
    Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
    Task<bool> RevokeTokenAsync(string refreshToken);
    Task<bool> RevokeJwtTokenAsync(string jwtToken);
    string GenerateJwtToken(UsuarioDto usuario);
    string GenerateRefreshToken();
}
