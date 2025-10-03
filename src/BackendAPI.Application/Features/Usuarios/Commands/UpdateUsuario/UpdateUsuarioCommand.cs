using AutoMapper;
using BackendAPI.Application.Common.Exceptions;
using BackendAPI.Domain.DTOs;
using BackendAPI.Domain.Entities;
using BackendAPI.Domain.Interfaces;
using MediatR;

namespace BackendAPI.Application.Features.Usuarios.Commands.UpdateUsuario;

public class UpdateUsuarioCommand : IRequest<UsuarioDto>
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string? Direccion { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public string? TipoDocumento { get; set; }
    public string? NumeroDocumento { get; set; }
}

public class UpdateUsuarioCommandHandler : IRequestHandler<UpdateUsuarioCommand, UsuarioDto>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IMapper _mapper;

    public UpdateUsuarioCommandHandler(IUsuarioRepository usuarioRepository, IMapper mapper)
    {
        _usuarioRepository = usuarioRepository;
        _mapper = mapper;
    }

    public async Task<UsuarioDto> Handle(UpdateUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(request.Id);
        if (usuario == null)
        {
            throw new NotFoundException(nameof(Usuario), request.Id);
        }

        // Verificar si el email ya existe en otro usuario
        if (await _usuarioRepository.EmailExistsAsync(request.Email, request.Id))
        {
            throw new ValidationException(new List<FluentValidation.Results.ValidationFailure>
            {
                new FluentValidation.Results.ValidationFailure("Email", "El email ya está registrado por otro usuario.")
            });
        }

        // Actualizar propiedades
        usuario.Nombre = request.Nombre;
        usuario.Apellido = request.Apellido;
        usuario.Email = request.Email;
        usuario.Telefono = request.Telefono;
        usuario.Direccion = request.Direccion;
        usuario.FechaNacimiento = request.FechaNacimiento;
        usuario.TipoDocumento = request.TipoDocumento;
        usuario.NumeroDocumento = request.NumeroDocumento;
        usuario.FechaModificacion = DateTime.UtcNow;

        await _usuarioRepository.UpdateAsync(usuario);
        return _mapper.Map<UsuarioDto>(usuario);
    }
}
