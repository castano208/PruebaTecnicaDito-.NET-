namespace BackendAPI.Domain.Entities;

public class UsuarioRol : BaseEntity
{
    public int UsuarioId { get; set; }
    public int RolId { get; set; }

    // Navigation properties
    public virtual Usuario Usuario { get; set; } = null!;
    public virtual Rol Rol { get; set; } = null!;
}
