namespace BackendAPI.Domain.DTOs;

public class PedidoDto
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public string NumeroPedido { get; set; } = string.Empty;
    public DateTime FechaPedido { get; set; }
    public string Estado { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public string? Comentarios { get; set; }
    public string? DireccionEntrega { get; set; }
    public DateTime FechaCreacion { get; set; }
    public bool Activo { get; set; }
    public UsuarioDto? Usuario { get; set; }
    public List<PedidoItemDto> PedidoItems { get; set; } = new();
}

public class CreatePedidoDto
{
    public int UsuarioId { get; set; }
    public string? Comentarios { get; set; }
    public string? DireccionEntrega { get; set; }
    public List<CreatePedidoItemDto> PedidoItems { get; set; } = new();
}

public class UpdatePedidoDto
{
    public string Estado { get; set; } = string.Empty;
    public string? Comentarios { get; set; }
    public string? DireccionEntrega { get; set; }
}

public class PedidoItemDto
{
    public int Id { get; set; }
    public int PedidoId { get; set; }
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
    public ProductoDto? Producto { get; set; }
}

public class CreatePedidoItemDto
{
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
}
