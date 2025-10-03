using System.ComponentModel.DataAnnotations;

namespace BackendAPI.Domain.Entities;

public class Rol : BaseEntity
{
    [Required]
    [StringLength(50)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Descripcion { get; set; }

    // Navigation properties
    public virtual ICollection<UsuarioRol> UsuarioRoles { get; set; } = new List<UsuarioRol>();
}
