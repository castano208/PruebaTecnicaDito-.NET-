using AutoMapper;
using BackendAPI.Application.Common.Interfaces;
using BackendAPI.Domain.DTOs;
using BackendAPI.Domain.Entities;
using BackendAPI.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BackendAPI.Application.Common.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public AuthService(IUsuarioRepository usuarioRepository, IMapper mapper, IConfiguration configuration)
    {
        _usuarioRepository = usuarioRepository;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        var usuario = await _usuarioRepository.GetByEmailAsync(loginDto.Email);
        if (usuario == null || !VerifyPassword(loginDto.Password, usuario.PasswordHash))
        {
            throw new UnauthorizedAccessException("Credenciales inválidas");
        }

        var token = GenerateJwtToken(_mapper.Map<UsuarioDto>(usuario));
        var refreshToken = GenerateRefreshToken();

        usuario.RefreshToken = refreshToken;
        usuario.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _usuarioRepository.UpdateAsync(usuario);

        return new AuthResponseDto
        {
            Token = token,
            RefreshToken = refreshToken,
            Expires = DateTime.UtcNow.AddHours(1),
            Usuario = _mapper.Map<UsuarioDto>(usuario)
        };
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        if (await _usuarioRepository.EmailExistsAsync(registerDto.Email))
        {
            throw new InvalidOperationException("El email ya está registrado");
        }

        var usuario = _mapper.Map<Usuario>(registerDto);
        usuario.PasswordHash = HashPassword(registerDto.Password);
        usuario.FechaCreacion = DateTime.UtcNow;
        usuario.Activo = true;

        var createdUsuario = await _usuarioRepository.AddAsync(usuario);
        var token = GenerateJwtToken(_mapper.Map<UsuarioDto>(createdUsuario));
        var refreshToken = GenerateRefreshToken();

        createdUsuario.RefreshToken = refreshToken;
        createdUsuario.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _usuarioRepository.UpdateAsync(createdUsuario);

        return new AuthResponseDto
        {
            Token = token,
            RefreshToken = refreshToken,
            Expires = DateTime.UtcNow.AddHours(1),
            Usuario = _mapper.Map<UsuarioDto>(createdUsuario)
        };
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
    {
        var usuario = await _usuarioRepository.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        if (usuario == null || usuario.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException("Token de actualización inválido");
        }

        var newToken = GenerateJwtToken(_mapper.Map<UsuarioDto>(usuario));
        var newRefreshToken = GenerateRefreshToken();

        usuario.RefreshToken = newRefreshToken;
        usuario.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _usuarioRepository.UpdateAsync(usuario);

        return new AuthResponseDto
        {
            Token = newToken,
            RefreshToken = newRefreshToken,
            Expires = DateTime.UtcNow.AddHours(1),
            Usuario = _mapper.Map<UsuarioDto>(usuario)
        };
    }

    public async Task<bool> RevokeTokenAsync(string refreshToken)
    {
        var usuario = await _usuarioRepository.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        if (usuario == null) return false;

        usuario.RefreshToken = null;
        usuario.RefreshTokenExpiryTime = null;
        await _usuarioRepository.UpdateAsync(usuario);
        return true;
    }

    public async Task<bool> RevokeJwtTokenAsync(string jwtToken)
    {
        try
        {
            // Extraer el ID del usuario del token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(jwtToken);
            var userIdClaim = token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                return false;

            // Obtener el usuario y limpiar sus tokens
            var usuario = await _usuarioRepository.GetByIdAsync(userId);
            if (usuario == null) return false;

            usuario.RefreshToken = null;
            usuario.RefreshTokenExpiryTime = null;
            await _usuarioRepository.UpdateAsync(usuario);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public string GenerateJwtToken(UsuarioDto usuario)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Name, $"{usuario.Nombre} {usuario.Apellido}")
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
