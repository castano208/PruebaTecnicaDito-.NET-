using BackendAPI.Domain.Entities;
using BackendAPI.Domain.Interfaces;
using BackendAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Infrastructure.Repositories;

public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
    {
        var query = _dbSet.Where(u => u.Email == email);
        
        if (excludeId.HasValue)
        {
            query = query.Where(u => u.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<IEnumerable<Usuario>> GetActiveUsersAsync()
    {
        return await _dbSet.Where(u => u.Activo).ToListAsync();
    }

    public IQueryable<Usuario> GetActiveUsersQueryable()
    {
        return _dbSet.Where(u => u.Activo);
    }
}
