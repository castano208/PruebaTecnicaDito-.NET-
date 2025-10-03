using BackendAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Producto> Productos { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<PedidoItem> PedidoItems { get; set; }
    public DbSet<Rol> Roles { get; set; }
    public DbSet<UsuarioRol> UsuarioRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Usuario configuration
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Apellido).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Telefono).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Direccion).HasMaxLength(500);
            entity.Property(e => e.TipoDocumento).HasMaxLength(50);
            entity.Property(e => e.NumeroDocumento).HasMaxLength(20);
            entity.Property(e => e.FechaCreacion).IsRequired();
            entity.Property(e => e.Activo).IsRequired().HasDefaultValue(true);

            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Producto configuration
        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Descripcion).HasMaxLength(1000);
            entity.Property(e => e.Precio).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.Stock).IsRequired();
            entity.Property(e => e.Categoria).HasMaxLength(100);
            entity.Property(e => e.Codigo).HasMaxLength(50);
            entity.Property(e => e.ImagenUrl).HasMaxLength(500);
            entity.Property(e => e.FechaCreacion).IsRequired();
            entity.Property(e => e.Activo).IsRequired().HasDefaultValue(true);

            entity.HasIndex(e => e.Codigo).IsUnique();
        });

        // Pedido configuration
        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UsuarioId).IsRequired();
            entity.Property(e => e.NumeroPedido).IsRequired().HasMaxLength(50);
            entity.Property(e => e.FechaPedido).IsRequired();
            entity.Property(e => e.Estado).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Total).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.Comentarios).HasMaxLength(500);
            entity.Property(e => e.DireccionEntrega).HasMaxLength(200);
            entity.Property(e => e.FechaCreacion).IsRequired();
            entity.Property(e => e.Activo).IsRequired().HasDefaultValue(true);

            entity.HasIndex(e => e.NumeroPedido).IsUnique();

            entity.HasOne(e => e.Usuario)
                .WithMany(u => u.Pedidos)
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // PedidoItem configuration
        modelBuilder.Entity<PedidoItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PedidoId).IsRequired();
            entity.Property(e => e.ProductoId).IsRequired();
            entity.Property(e => e.Cantidad).IsRequired();
            entity.Property(e => e.PrecioUnitario).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.Subtotal).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.FechaCreacion).IsRequired();
            entity.Property(e => e.Activo).IsRequired().HasDefaultValue(true);

            entity.HasOne(e => e.Pedido)
                .WithMany(p => p.PedidoItems)
                .HasForeignKey(e => e.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Producto)
                .WithMany(p => p.PedidoItems)
                .HasForeignKey(e => e.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Rol configuration
        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Descripcion).HasMaxLength(200);
            entity.Property(e => e.FechaCreacion).IsRequired();
            entity.Property(e => e.Activo).IsRequired().HasDefaultValue(true);

            entity.HasIndex(e => e.Nombre).IsUnique();
        });

        // UsuarioRol configuration
        modelBuilder.Entity<UsuarioRol>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UsuarioId).IsRequired();
            entity.Property(e => e.RolId).IsRequired();
            entity.Property(e => e.FechaCreacion).IsRequired();
            entity.Property(e => e.Activo).IsRequired().HasDefaultValue(true);

            entity.HasOne(e => e.Usuario)
                .WithMany(u => u.UsuarioRoles)
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Rol)
                .WithMany(r => r.UsuarioRoles)
                .HasForeignKey(e => e.RolId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.UsuarioId, e.RolId }).IsUnique();
        });
    }
}
