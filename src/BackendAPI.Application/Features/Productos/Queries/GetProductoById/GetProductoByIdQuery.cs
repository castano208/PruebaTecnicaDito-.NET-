using AutoMapper;
using BackendAPI.Application.Common.Exceptions;
using BackendAPI.Domain.DTOs;
using BackendAPI.Domain.Entities;
using BackendAPI.Domain.Interfaces;
using MediatR;

namespace BackendAPI.Application.Features.Productos.Queries.GetProductoById;

public class GetProductoByIdQuery : IRequest<ProductoDto>
{
    public int Id { get; set; }
}

public class GetProductoByIdQueryHandler : IRequestHandler<GetProductoByIdQuery, ProductoDto>
{
    private readonly IProductoRepository _productoRepository;
    private readonly IMapper _mapper;

    public GetProductoByIdQueryHandler(IProductoRepository productoRepository, IMapper mapper)
    {
        _productoRepository = productoRepository;
        _mapper = mapper;
    }

    public async Task<ProductoDto> Handle(GetProductoByIdQuery request, CancellationToken cancellationToken)
    {
        var producto = await _productoRepository.GetByIdAsync(request.Id);
        if (producto == null)
        {
            throw new NotFoundException(nameof(Producto), request.Id);
        }

        return _mapper.Map<ProductoDto>(producto);
    }
}
