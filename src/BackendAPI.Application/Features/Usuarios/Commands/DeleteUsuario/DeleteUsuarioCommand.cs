using BackendAPI.Application.Common.Exceptions;
using BackendAPI.Domain.Entities;
using BackendAPI.Domain.Interfaces;
using MediatR;

namespace BackendAPI.Application.Features.Usuarios.Commands.DeleteUsuario;

public class DeleteUsuarioCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteUsuarioCommandHandler : IRequestHandler<DeleteUsuarioCommand>
{
    private readonly IUsuarioRepository _usuarioRepository;

    public DeleteUsuarioCommandHandler(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task Handle(DeleteUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(request.Id);
        if (usuario == null)
        {
            throw new NotFoundException(nameof(Usuario), request.Id);
        }

        // Soft delete - marcar como inactivo
        usuario.Activo = false;
        usuario.FechaModificacion = DateTime.UtcNow;
        await _usuarioRepository.UpdateAsync(usuario);
    }
}
