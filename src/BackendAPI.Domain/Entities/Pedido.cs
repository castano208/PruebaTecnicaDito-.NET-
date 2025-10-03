using System.ComponentModel.DataAnnotations;

namespace BackendAPI.Domain.Entities;

public class Pedido : BaseEntity
{
    [Required]
    public int UsuarioId { get; set; }

    [Required]
    [StringLength(50)]
    public string NumeroPedido { get; set; } = string.Empty;

    [Required]
    public DateTime FechaPedido { get; set; }

    [Required]
    [StringLength(50)]
    public string Estado { get; set; } = "Pendiente";

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "El total debe ser mayor a 0")]
    public decimal Total { get; set; }

    [StringLength(500)]
    public string? Comentarios { get; set; }

    [StringLength(200)]
    public string? DireccionEntrega { get; set; }

    // Navigation properties
    public virtual Usuario Usuario { get; set; } = null!;
    public virtual ICollection<PedidoItem> PedidoItems { get; set; } = new List<PedidoItem>();
}
