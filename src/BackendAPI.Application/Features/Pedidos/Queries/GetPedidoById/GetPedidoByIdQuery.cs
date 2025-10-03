using AutoMapper;
using BackendAPI.Application.Common.Exceptions;
using BackendAPI.Domain.DTOs;
using BackendAPI.Domain.Entities;
using BackendAPI.Domain.Interfaces;
using MediatR;

namespace BackendAPI.Application.Features.Pedidos.Queries.GetPedidoById;

public class GetPedidoByIdQuery : IRequest<PedidoDto>
{
    public int Id { get; set; }
}

public class GetPedidoByIdQueryHandler : IRequestHandler<GetPedidoByIdQuery, PedidoDto>
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IMapper _mapper;

    public GetPedidoByIdQueryHandler(IPedidoRepository pedidoRepository, IMapper mapper)
    {
        _pedidoRepository = pedidoRepository;
        _mapper = mapper;
    }

    public async Task<PedidoDto> Handle(GetPedidoByIdQuery request, CancellationToken cancellationToken)
    {
        var pedido = await _pedidoRepository.GetByIdAsync(request.Id);
        if (pedido == null)
        {
            throw new NotFoundException(nameof(Pedido), request.Id);
        }

        return _mapper.Map<PedidoDto>(pedido);
    }
}
