using System.ComponentModel.DataAnnotations;

namespace BackendAPI.Domain.Entities;

public class Producto : BaseEntity
{
    [Required]
    [StringLength(200)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Descripcion { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
    public decimal Precio { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
    public int Stock { get; set; }

    [StringLength(100)]
    public string? Categoria { get; set; }

    [StringLength(50)]
    public string? Codigo { get; set; }

    [StringLength(500)]
    public string? ImagenUrl { get; set; }

    // Navigation properties
    public virtual ICollection<PedidoItem> PedidoItems { get; set; } = new List<PedidoItem>();
}
