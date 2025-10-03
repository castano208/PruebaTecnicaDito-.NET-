using BackendAPI.Domain.Entities;
using BackendAPI.Domain.Interfaces;
using BackendAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Infrastructure.Repositories;

public class ProductoRepository : Repository<Producto>, IProductoRepository
{
    public ProductoRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Producto>> GetByCategoriaAsync(string categoria)
    {
        return await _dbSet
            .Where(p => p.Categoria == categoria && p.Activo)
            .ToListAsync();
    }

    public async Task<IEnumerable<Producto>> GetAvailableProductsAsync()
    {
        return await _dbSet
            .Where(p => p.Activo && p.Stock > 0)
            .ToListAsync();
    }

    public async Task<bool> CodigoExistsAsync(string codigo, int? excludeId = null)
    {
        var query = _dbSet.Where(p => p.Codigo == codigo);
        
        if (excludeId.HasValue)
        {
            query = query.Where(p => p.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<IEnumerable<Producto>> SearchByNameAsync(string name)
    {
        return await _dbSet
            .Where(p => p.Activo && p.Nombre.Contains(name))
            .ToListAsync();
    }
}
