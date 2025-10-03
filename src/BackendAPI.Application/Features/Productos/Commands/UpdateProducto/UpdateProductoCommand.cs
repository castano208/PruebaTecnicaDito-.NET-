using AutoMapper;
using BackendAPI.Application.Common.Exceptions;
using BackendAPI.Domain.DTOs;
using BackendAPI.Domain.Entities;
using BackendAPI.Domain.Interfaces;
using MediatR;

namespace BackendAPI.Application.Features.Productos.Commands.UpdateProducto;

public class UpdateProductoCommand : IRequest<ProductoDto>
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public string? Categoria { get; set; }
    public string? Codigo { get; set; }
    public string? ImagenUrl { get; set; }
}

public class UpdateProductoCommandHandler : IRequestHandler<UpdateProductoCommand, ProductoDto>
{
    private readonly IProductoRepository _productoRepository;
    private readonly IMapper _mapper;

    public UpdateProductoCommandHandler(IProductoRepository productoRepository, IMapper mapper)
    {
        _productoRepository = productoRepository;
        _mapper = mapper;
    }

    public async Task<ProductoDto> Handle(UpdateProductoCommand request, CancellationToken cancellationToken)
    {
        var producto = await _productoRepository.GetByIdAsync(request.Id);
        if (producto == null)
        {
            throw new NotFoundException(nameof(Producto), request.Id);
        }

        // Verificar si el código ya existe en otro producto
        if (!string.IsNullOrEmpty(request.Codigo) && await _productoRepository.CodigoExistsAsync(request.Codigo, request.Id))
        {
            throw new ValidationException(new List<FluentValidation.Results.ValidationFailure>
            {
                new FluentValidation.Results.ValidationFailure("Codigo", "El código del producto ya existe en otro producto.")
            });
        }

        // Actualizar propiedades
        producto.Nombre = request.Nombre;
        producto.Descripcion = request.Descripcion;
        producto.Precio = request.Precio;
        producto.Stock = request.Stock;
        producto.Categoria = request.Categoria;
        producto.Codigo = request.Codigo;
        producto.ImagenUrl = request.ImagenUrl;
        producto.FechaModificacion = DateTime.UtcNow;

        await _productoRepository.UpdateAsync(producto);
        return _mapper.Map<ProductoDto>(producto);
    }
}
