using AutoMapper;
using BackendAPI.Domain.DTOs;
using BackendAPI.Domain.Entities;

namespace BackendAPI.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Usuario mappings
        CreateMap<Usuario, UsuarioDto>();
        CreateMap<CreateUsuarioDto, Usuario>();
        CreateMap<UpdateUsuarioDto, Usuario>();

        // Producto mappings
        CreateMap<Producto, ProductoDto>();
        CreateMap<CreateProductoDto, Producto>();
        CreateMap<UpdateProductoDto, Producto>();

        // Pedido mappings
        CreateMap<Pedido, PedidoDto>();
        CreateMap<CreatePedidoDto, Pedido>();
        CreateMap<UpdatePedidoDto, Pedido>();

        // PedidoItem mappings
        CreateMap<PedidoItem, PedidoItemDto>();
        CreateMap<CreatePedidoItemDto, PedidoItem>();

        // Auth mappings
        CreateMap<RegisterDto, Usuario>();
        CreateMap<Usuario, UsuarioDto>();
        
        // Command mappings
        CreateMap<BackendAPI.Application.Features.Usuarios.Commands.CreateUsuario.CreateUsuarioCommand, Usuario>();
        CreateMap<BackendAPI.Application.Features.Productos.Commands.CreateProducto.CreateProductoCommand, Producto>();
    }
}
