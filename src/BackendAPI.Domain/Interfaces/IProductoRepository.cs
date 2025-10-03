using BackendAPI.Domain.Entities;

namespace BackendAPI.Domain.Interfaces;

public interface IProductoRepository : IRepository<Producto>
{
    Task<IEnumerable<Producto>> GetByCategoriaAsync(string categoria);
    Task<IEnumerable<Producto>> GetAvailableProductsAsync();
    Task<bool> CodigoExistsAsync(string codigo, int? excludeId = null);
    Task<IEnumerable<Producto>> SearchByNameAsync(string name);
}
