using AutoMapper;
using BackendAPI.Domain.DTOs;
using BackendAPI.Domain.Entities;
using BackendAPI.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Application.Features.Usuarios.Queries.GetUsuarios;

public class GetUsuariosQuery : IRequest<PagedResultDto<UsuarioDto>>
{
    public PaginationDto Pagination { get; set; } = new();
}

public class GetUsuariosQueryHandler : IRequestHandler<GetUsuariosQuery, PagedResultDto<UsuarioDto>>
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IMapper _mapper;

    public GetUsuariosQueryHandler(IUsuarioRepository usuarioRepository, IMapper mapper)
    {
        _usuarioRepository = usuarioRepository;
        _mapper = mapper;
    }

    public async Task<PagedResultDto<UsuarioDto>> Handle(GetUsuariosQuery request, CancellationToken cancellationToken)
    {
        var query = _usuarioRepository.GetActiveUsersQueryable();

        // Aplicar filtro de búsqueda
        if (!string.IsNullOrEmpty(request.Pagination.Search))
        {
            query = query.Where(u => 
                u.Nombre.Contains(request.Pagination.Search) ||
                u.Apellido.Contains(request.Pagination.Search) ||
                u.Email.Contains(request.Pagination.Search));
        }

        // Aplicar ordenamiento
        if (!string.IsNullOrEmpty(request.Pagination.SortBy))
        {
            query = request.Pagination.SortBy.ToLower() switch
            {
                "nombre" => request.Pagination.SortDescending ? query.OrderByDescending(u => u.Nombre) : query.OrderBy(u => u.Nombre),
                "apellido" => request.Pagination.SortDescending ? query.OrderByDescending(u => u.Apellido) : query.OrderBy(u => u.Apellido),
                "email" => request.Pagination.SortDescending ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
                "fechacreacion" => request.Pagination.SortDescending ? query.OrderByDescending(u => u.FechaCreacion) : query.OrderBy(u => u.FechaCreacion),
                _ => query.OrderBy(u => u.Nombre)
            };
        }
        else
        {
            query = query.OrderBy(u => u.Nombre);
        }

        // Obtener total de registros
        var totalCount = await query.CountAsync();

        // Aplicar paginación
        var usuarios = await query
            .Skip((request.Pagination.Page - 1) * request.Pagination.PageSize)
            .Take(request.Pagination.PageSize)
            .ToListAsync();

        return new PagedResultDto<UsuarioDto>
        {
            Data = _mapper.Map<List<UsuarioDto>>(usuarios),
            TotalCount = totalCount,
            Page = request.Pagination.Page,
            PageSize = request.Pagination.PageSize
        };
    }
}
