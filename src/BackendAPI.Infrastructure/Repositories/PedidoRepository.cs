using BackendAPI.Domain.Entities;
using BackendAPI.Domain.Interfaces;
using BackendAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Infrastructure.Repositories;

public class PedidoRepository : Repository<Pedido>, IPedidoRepository
{
    public PedidoRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Pedido>> GetByUsuarioIdAsync(int usuarioId)
    {
        return await _dbSet
            .Where(p => p.UsuarioId == usuarioId && p.Activo)
            .Include(p => p.Usuario)
            .Include(p => p.PedidoItems)
            .ThenInclude(pi => pi.Producto)
            .ToListAsync();
    }

    public async Task<Pedido?> GetByNumeroPedidoAsync(string numeroPedido)
    {
        return await _dbSet
            .Include(p => p.Usuario)
            .Include(p => p.PedidoItems)
            .ThenInclude(pi => pi.Producto)
            .FirstOrDefaultAsync(p => p.NumeroPedido == numeroPedido);
    }

    public async Task<IEnumerable<Pedido>> GetByEstadoAsync(string estado)
    {
        return await _dbSet
            .Where(p => p.Estado == estado && p.Activo)
            .Include(p => p.Usuario)
            .Include(p => p.PedidoItems)
            .ThenInclude(pi => pi.Producto)
            .ToListAsync();
    }

    public async Task<IEnumerable<Pedido>> GetByDateRangeAsync(DateTime fechaInicio, DateTime fechaFin)
    {
        return await _dbSet
            .Where(p => p.FechaPedido >= fechaInicio && p.FechaPedido <= fechaFin && p.Activo)
            .Include(p => p.Usuario)
            .Include(p => p.PedidoItems)
            .ThenInclude(pi => pi.Producto)
            .ToListAsync();
    }

    public async Task<string> GenerateNextNumeroPedidoAsync()
    {
        var lastPedido = await _dbSet
            .OrderByDescending(p => p.Id)
            .FirstOrDefaultAsync();

        if (lastPedido == null)
        {
            return "PED-000001";
        }

        var lastNumber = int.Parse(lastPedido.NumeroPedido.Split('-')[1]);
        return $"PED-{(lastNumber + 1):D6}";
    }
}
