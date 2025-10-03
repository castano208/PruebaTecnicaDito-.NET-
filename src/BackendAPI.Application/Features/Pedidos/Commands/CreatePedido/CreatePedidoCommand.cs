using AutoMapper;
using BackendAPI.Application.Common.Exceptions;
using BackendAPI.Domain.DTOs;
using BackendAPI.Domain.Entities;
using BackendAPI.Domain.Interfaces;
using MediatR;

namespace BackendAPI.Application.Features.Pedidos.Commands.CreatePedido;

public class CreatePedidoCommand : IRequest<PedidoDto>
{
    public int UsuarioId { get; set; }
    public string? Comentarios { get; set; }
    public string? DireccionEntrega { get; set; }
    public List<CreatePedidoItemDto> PedidoItems { get; set; } = new();
}

public class CreatePedidoCommandHandler : IRequestHandler<CreatePedidoCommand, PedidoDto>
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IProductoRepository _productoRepository;
    private readonly IMapper _mapper;

    public CreatePedidoCommandHandler(
        IPedidoRepository pedidoRepository,
        IUsuarioRepository usuarioRepository,
        IProductoRepository productoRepository,
        IMapper mapper)
    {
        _pedidoRepository = pedidoRepository;
        _usuarioRepository = usuarioRepository;
        _productoRepository = productoRepository;
        _mapper = mapper;
    }

    public async Task<PedidoDto> Handle(CreatePedidoCommand request, CancellationToken cancellationToken)
    {
        // Verificar que el usuario existe
        var usuario = await _usuarioRepository.GetByIdAsync(request.UsuarioId);
        if (usuario == null)
        {
            throw new NotFoundException(nameof(Usuario), request.UsuarioId);
        }

        // Verificar que todos los productos existen y tienen stock suficiente
        var productos = new List<Producto>();
        foreach (var item in request.PedidoItems)
        {
            var producto = await _productoRepository.GetByIdAsync(item.ProductoId);
            if (producto == null)
            {
                throw new NotFoundException(nameof(Producto), item.ProductoId);
            }

            if (producto.Stock < item.Cantidad)
            {
                throw new ValidationException(new List<FluentValidation.Results.ValidationFailure>
                {
                    new FluentValidation.Results.ValidationFailure("Stock", $"Stock insuficiente para el producto {producto.Nombre}. Stock disponible: {producto.Stock}")
                });
            }

            productos.Add(producto);
        }

        // Crear el pedido
        var pedido = new Pedido
        {
            UsuarioId = request.UsuarioId,
            NumeroPedido = await _pedidoRepository.GenerateNextNumeroPedidoAsync(),
            FechaPedido = DateTime.UtcNow,
            Estado = "Pendiente",
            Comentarios = request.Comentarios,
            DireccionEntrega = request.DireccionEntrega,
            FechaCreacion = DateTime.UtcNow,
            Activo = true
        };

        // Crear los items del pedido y calcular el total
        decimal total = 0;
        foreach (var item in request.PedidoItems)
        {
            var producto = productos.First(p => p.Id == item.ProductoId);
            var subtotal = producto.Precio * item.Cantidad;
            total += subtotal;

            var pedidoItem = new PedidoItem
            {
                PedidoId = pedido.Id,
                ProductoId = item.ProductoId,
                Cantidad = item.Cantidad,
                PrecioUnitario = producto.Precio,
                Subtotal = subtotal,
                FechaCreacion = DateTime.UtcNow,
                Activo = true
            };

            pedido.PedidoItems.Add(pedidoItem);
        }

        pedido.Total = total;

        // Actualizar el stock de los productos
        foreach (var item in request.PedidoItems)
        {
            var producto = productos.First(p => p.Id == item.ProductoId);
            producto.Stock -= item.Cantidad;
            await _productoRepository.UpdateAsync(producto);
        }

        var createdPedido = await _pedidoRepository.AddAsync(pedido);
        return _mapper.Map<PedidoDto>(createdPedido);
    }
}
