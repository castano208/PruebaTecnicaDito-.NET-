using BackendAPI.Application.Common.Exceptions;
using BackendAPI.Domain.Entities;
using BackendAPI.Domain.Interfaces;
using MediatR;

namespace BackendAPI.Application.Features.Productos.Commands.DeleteProducto;

public class DeleteProductoCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteProductoCommandHandler : IRequestHandler<DeleteProductoCommand>
{
    private readonly IProductoRepository _productoRepository;

    public DeleteProductoCommandHandler(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task Handle(DeleteProductoCommand request, CancellationToken cancellationToken)
    {
        var producto = await _productoRepository.GetByIdAsync(request.Id);
        if (producto == null)
        {
            throw new NotFoundException(nameof(Producto), request.Id);
        }

        // Soft delete - marcar como inactivo
        producto.Activo = false;
        producto.FechaModificacion = DateTime.UtcNow;
        await _productoRepository.UpdateAsync(producto);
    }
}
