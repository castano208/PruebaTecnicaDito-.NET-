using AutoMapper;
using BackendAPI.Domain.DTOs;
using BackendAPI.Domain.Interfaces;
using MediatR;

namespace BackendAPI.Application.Features.Productos.Queries.GetProductos;

public class GetProductosQuery : IRequest<IEnumerable<ProductoDto>>
{
}

public class GetProductosQueryHandler : IRequestHandler<GetProductosQuery, IEnumerable<ProductoDto>>
{
    private readonly IProductoRepository _productoRepository;
    private readonly IMapper _mapper;

    public GetProductosQueryHandler(IProductoRepository productoRepository, IMapper mapper)
    {
        _productoRepository = productoRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductoDto>> Handle(GetProductosQuery request, CancellationToken cancellationToken)
    {
        var productos = await _productoRepository.GetAvailableProductsAsync();
        return _mapper.Map<IEnumerable<ProductoDto>>(productos);
    }
}
