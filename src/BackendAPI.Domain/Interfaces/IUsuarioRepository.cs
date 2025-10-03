using BackendAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Domain.Interfaces;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Task<Usuario?> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email, int? excludeId = null);
    Task<IEnumerable<Usuario>> GetActiveUsersAsync();
    IQueryable<Usuario> GetActiveUsersQueryable();
}
