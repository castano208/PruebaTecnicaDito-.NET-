using System.ComponentModel.DataAnnotations;

namespace BackendAPI.Domain.Entities;

public class PedidoItem : BaseEntity
{
    [Required]
    public int PedidoId { get; set; }

    [Required]
    public int ProductoId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
    public int Cantidad { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
    public decimal PrecioUnitario { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "El subtotal debe ser mayor a 0")]
    public decimal Subtotal { get; set; }

    // Navigation properties
    public virtual Pedido Pedido { get; set; } = null!;
    public virtual Producto Producto { get; set; } = null!;
}
