using BackendAPI.Domain.Entities;

namespace BackendAPI.Domain.Interfaces;

public interface IPedidoRepository : IRepository<Pedido>
{
    Task<IEnumerable<Pedido>> GetByUsuarioIdAsync(int usuarioId);
    Task<Pedido?> GetByNumeroPedidoAsync(string numeroPedido);
    Task<IEnumerable<Pedido>> GetByEstadoAsync(string estado);
    Task<IEnumerable<Pedido>> GetByDateRangeAsync(DateTime fechaInicio, DateTime fechaFin);
    Task<string> GenerateNextNumeroPedidoAsync();
}
