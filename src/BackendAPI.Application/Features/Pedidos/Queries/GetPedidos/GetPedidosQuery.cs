using AutoMapper;
using BackendAPI.Domain.DTOs;
using BackendAPI.Domain.Interfaces;
using MediatR;

namespace BackendAPI.Application.Features.Pedidos.Queries.GetPedidos;

public class GetPedidosQuery : IRequest<IEnumerable<PedidoDto>>
{
}

public class GetPedidosQueryHandler : IRequestHandler<GetPedidosQuery, IEnumerable<PedidoDto>>
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IMapper _mapper;

    public GetPedidosQueryHandler(IPedidoRepository pedidoRepository, IMapper mapper)
    {
        _pedidoRepository = pedidoRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PedidoDto>> Handle(GetPedidosQuery request, CancellationToken cancellationToken)
    {
        var pedidos = await _pedidoRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<PedidoDto>>(pedidos);
    }
}
