using AutoMapper;
using BackendAPI.Application.Common.Exceptions;
using BackendAPI.Domain.DTOs;
using BackendAPI.Domain.Entities;
using BackendAPI.Domain.Interfaces;
using MediatR;

namespace BackendAPI.Application.Features.Productos.Commands.CreateProducto;

public class CreateProductoCommand : IRequest<ProductoDto>
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public string? Categoria { get; set; }
    public string? Codigo { get; set; }
    public string? ImagenUrl { get; set; }
}

public class CreateProductoCommandHandler : IRequestHandler<CreateProductoCommand, ProductoDto>
{
    private readonly IProductoRepository _productoRepository;
    private readonly IMapper _mapper;

    public CreateProductoCommandHandler(IProductoRepository productoRepository, IMapper mapper)
    {
        _productoRepository = productoRepository;
        _mapper = mapper;
    }

    public async Task<ProductoDto> Handle(CreateProductoCommand request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(request.Codigo) && await _productoRepository.CodigoExistsAsync(request.Codigo))
        {
            throw new ValidationException(new List<FluentValidation.Results.ValidationFailure>
            {
                new FluentValidation.Results.ValidationFailure("Codigo", "El código del producto ya existe.")
            });
        }

        var producto = _mapper.Map<Producto>(request);
        producto.FechaCreacion = DateTime.UtcNow;
        producto.Activo = true;

        var createdProducto = await _productoRepository.AddAsync(producto);
        return _mapper.Map<ProductoDto>(createdProducto);
    }
}
