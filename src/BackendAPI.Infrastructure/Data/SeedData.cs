using BackendAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace BackendAPI.Infrastructure.Data;

public static class SeedData
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Ensure database is created
        await context.Database.EnsureCreatedAsync();

        // Clear all data in correct order to respect foreign key constraints
        // 1. Clear dependent entities first
        var existingPedidoItems = await context.PedidoItems.ToListAsync();
        if (existingPedidoItems.Any())
        {
            context.PedidoItems.RemoveRange(existingPedidoItems);
            await context.SaveChangesAsync();
        }

        var existingPedidos = await context.Pedidos.ToListAsync();
        if (existingPedidos.Any())
        {
            context.Pedidos.RemoveRange(existingPedidos);
            await context.SaveChangesAsync();
        }

        var existingUsuarioRoles = await context.UsuarioRoles.ToListAsync();
        if (existingUsuarioRoles.Any())
        {
            context.UsuarioRoles.RemoveRange(existingUsuarioRoles);
            await context.SaveChangesAsync();
        }

        var existingUsuarios = await context.Usuarios.ToListAsync();
        if (existingUsuarios.Any())
        {
            context.Usuarios.RemoveRange(existingUsuarios);
            await context.SaveChangesAsync();
        }

        var existingProductos = await context.Productos.ToListAsync();
        if (existingProductos.Any())
        {
            context.Productos.RemoveRange(existingProductos);
            await context.SaveChangesAsync();
        }

        // 2. Clear parent entities last
        var existingRoles = await context.Roles.ToListAsync();
        if (existingRoles.Any())
        {
            context.Roles.RemoveRange(existingRoles);
            await context.SaveChangesAsync();
        }
        {
            var roles = new List<Rol>
            {
                new() { Id = 1, Nombre = "Administrador", Descripcion = "Administrador del sistema", FechaCreacion = DateTime.UtcNow, Activo = true },
                new() { Id = 2, Nombre = "Usuario", Descripcion = "Usuario estándar", FechaCreacion = DateTime.UtcNow, Activo = true },
                new() { Id = 3, Nombre = "Vendedor", Descripcion = "Vendedor de productos", FechaCreacion = DateTime.UtcNow, Activo = true }
            };

            await context.Roles.AddRangeAsync(roles);
        }

        // Seed Usuarios
        {
            var usuarios = new List<Usuario>
            {
                new() 
                { 
                    Id = 1, 
                    Nombre = "Santiago", 
                    Apellido = "Henao Castaño", 
                    Email = "zsantiagohenao@gmail.com", 
                    Telefono = "1035418250",
                    Direccion = "Colombia",
                    TipoDocumento = "CC",
                    NumeroDocumento = "1035418250",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    FechaNacimiento = new DateTime(2005, 11, 17),
                    FechaCreacion = DateTime.UtcNow,
                    Activo = true
                },
                new() 
                { 
                    Id = 2, 
                    Nombre = "María Elena", 
                    Apellido = "García", 
                    Email = "maria@empresa.com", 
                    Telefono = "3002345678",
                    Direccion = "Carrera 45 #78-90, Medellín",
                    TipoDocumento = "CC",
                    NumeroDocumento = "87654321",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Maria123!"),
                    FechaNacimiento = new DateTime(1990, 8, 22),
                    FechaCreacion = DateTime.UtcNow,
                    Activo = true
                },
                new() 
                { 
                    Id = 3, 
                    Nombre = "Carlos Alberto", 
                    Apellido = "Rodríguez", 
                    Email = "carlos@empresa.com", 
                    Telefono = "3003456789",
                    Direccion = "Avenida 5 #12-34, Cali",
                    TipoDocumento = "CC",
                    NumeroDocumento = "11223344",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Carlos123!"),
                    FechaNacimiento = new DateTime(1988, 12, 10),
                    FechaCreacion = DateTime.UtcNow,
                    Activo = true
                },
                new() 
                { 
                    Id = 4, 
                    Nombre = "Ana Lucía", 
                    Apellido = "Martínez", 
                    Email = "ana@empresa.com", 
                    Telefono = "3004567890",
                    Direccion = "Calle 80 #15-20, Barranquilla",
                    TipoDocumento = "CC",
                    NumeroDocumento = "55667788",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Ana123!"),
                    FechaNacimiento = new DateTime(1992, 3, 5),
                    FechaCreacion = DateTime.UtcNow,
                    Activo = true
                },
                new() 
                { 
                    Id = 5, 
                    Nombre = "Pedro José", 
                    Apellido = "López", 
                    Email = "pedro@empresa.com", 
                    Telefono = "3005678901",
                    Direccion = "Carrera 7 #25-30, Bucaramanga",
                    TipoDocumento = "CC",
                    NumeroDocumento = "99887766",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Pedro123!"),
                    FechaNacimiento = new DateTime(1987, 7, 18),
                    FechaCreacion = DateTime.UtcNow,
                    Activo = true
                }
            };

            await context.Usuarios.AddRangeAsync(usuarios);
        }

        // Seed UsuarioRoles
        {
            var usuarioRoles = new List<UsuarioRol>
            {
                new() { Id = 1, UsuarioId = 1, RolId = 1, FechaCreacion = DateTime.UtcNow, Activo = true }, // Juan Carlos - Administrador
                new() { Id = 2, UsuarioId = 2, RolId = 2, FechaCreacion = DateTime.UtcNow, Activo = true }, // María Elena - Usuario
                new() { Id = 3, UsuarioId = 3, RolId = 3, FechaCreacion = DateTime.UtcNow, Activo = true }, // Carlos Alberto - Vendedor
                new() { Id = 4, UsuarioId = 4, RolId = 2, FechaCreacion = DateTime.UtcNow, Activo = true }, // Ana Lucía - Usuario
                new() { Id = 5, UsuarioId = 5, RolId = 3, FechaCreacion = DateTime.UtcNow, Activo = true }  // Pedro José - Vendedor
            };

            await context.UsuarioRoles.AddRangeAsync(usuarioRoles);
        }

        // Seed Productos
        {
            var productos = new List<Producto>
            {
                new() 
                { 
                    Id = 1, 
                    Nombre = "Laptop Dell Inspiron 15", 
                    Descripcion = "Laptop Dell Inspiron 15 3000 con procesador Intel Core i5, 8GB RAM, 256GB SSD",
                    Precio = 2500000m,
                    Stock = 15,
                    Categoria = "Electrónicos",
                    Codigo = "DELL-INS15-001",
                    ImagenUrl = "https://example.com/images/dell-inspiron-15.jpg",
                    FechaCreacion = DateTime.UtcNow,
                    Activo = true
                },
                new() 
                { 
                    Id = 2, 
                    Nombre = "Smartphone Samsung Galaxy A54", 
                    Descripcion = "Smartphone Samsung Galaxy A54 5G con 128GB de almacenamiento y cámara de 50MP",
                    Precio = 1800000m,
                    Stock = 25,
                    Categoria = "Electrónicos",
                    Codigo = "SAMS-A54-001",
                    ImagenUrl = "https://example.com/images/samsung-galaxy-a54.jpg",
                    FechaCreacion = DateTime.UtcNow,
                    Activo = true
                },
                new() 
                { 
                    Id = 3, 
                    Nombre = "Mesa de Oficina Ejecutiva", 
                    Descripcion = "Mesa de oficina ejecutiva en madera de roble con cajones y espacio para computadora",
                    Precio = 850000m,
                    Stock = 8,
                    Categoria = "Muebles",
                    Codigo = "MESA-OF-001",
                    ImagenUrl = "https://example.com/images/mesa-oficina-ejecutiva.jpg",
                    FechaCreacion = DateTime.UtcNow,
                    Activo = true
                },
                new() 
                { 
                    Id = 4, 
                    Nombre = "Silla Ergonómica Profesional", 
                    Descripcion = "Silla ergonómica profesional con soporte lumbar y reposabrazos ajustables",
                    Precio = 450000m,
                    Stock = 20,
                    Categoria = "Muebles",
                    Codigo = "SILLA-ERG-001",
                    ImagenUrl = "https://example.com/images/silla-ergonomica.jpg",
                    FechaCreacion = DateTime.UtcNow,
                    Activo = true
                },
                new() 
                { 
                    Id = 5, 
                    Nombre = "Monitor LG UltraWide 29", 
                    Descripcion = "Monitor LG UltraWide 29 pulgadas con resolución 2560x1080 y tecnología IPS",
                    Precio = 1200000m,
                    Stock = 12,
                    Categoria = "Electrónicos",
                    Codigo = "LG-UW29-001",
                    ImagenUrl = "https://example.com/images/lg-ultrawide-29.jpg",
                    FechaCreacion = DateTime.UtcNow,
                    Activo = true
                },
                new() 
                { 
                    Id = 6, 
                    Nombre = "Teclado Mecánico Logitech", 
                    Descripcion = "Teclado mecánico Logitech G Pro con switches táctiles y retroiluminación RGB",
                    Precio = 350000m,
                    Stock = 30,
                    Categoria = "Accesorios",
                    Codigo = "LOG-GPRO-001",
                    ImagenUrl = "https://example.com/images/teclado-mecanico-logitech.jpg",
                    FechaCreacion = DateTime.UtcNow,
                    Activo = true
                },
                new() 
                { 
                    Id = 7, 
                    Nombre = "Mouse Inalámbrico Microsoft", 
                    Descripcion = "Mouse inalámbrico Microsoft Surface con sensor óptico de alta precisión",
                    Precio = 180000m,
                    Stock = 40,
                    Categoria = "Accesorios",
                    Codigo = "MS-SURF-001",
                    ImagenUrl = "https://example.com/images/mouse-microsoft-surface.jpg",
                    FechaCreacion = DateTime.UtcNow,
                    Activo = true
                },
                new() 
                { 
                    Id = 8, 
                    Nombre = "Impresora HP LaserJet Pro", 
                    Descripcion = "Impresora HP LaserJet Pro M404n con impresión a doble cara automática",
                    Precio = 650000m,
                    Stock = 6,
                    Categoria = "Oficina",
                    Codigo = "HP-LJ-M404-001",
                    ImagenUrl = "https://example.com/images/hp-laserjet-pro.jpg",
                    FechaCreacion = DateTime.UtcNow,
                    Activo = true
                },
                new() 
                { 
                    Id = 9, 
                    Nombre = "Router WiFi 6 TP-Link", 
                    Descripcion = "Router WiFi 6 TP-Link Archer AX73 con velocidades hasta 5.4 Gbps",
                    Precio = 420000m,
                    Stock = 10,
                    Categoria = "Redes",
                    Codigo = "TPL-AX73-001",
                    ImagenUrl = "https://example.com/images/tp-link-ax73.jpg",
                    FechaCreacion = DateTime.UtcNow,
                    Activo = true
                },
                new() 
                { 
                    Id = 10, 
                    Nombre = "Disco Duro Externo Seagate 2TB", 
                    Descripcion = "Disco duro externo Seagate Backup Plus de 2TB con conexión USB 3.0",
                    Precio = 280000m,
                    Stock = 18,
                    Categoria = "Almacenamiento",
                    Codigo = "SEAG-2TB-001",
                    ImagenUrl = "https://example.com/images/seagate-2tb.jpg",
                    FechaCreacion = DateTime.UtcNow,
                    Activo = true
                }
            };

            await context.Productos.AddRangeAsync(productos);
        }

        // Seed Pedidos
        {
            var pedidos = new List<Pedido>
            {
                new() 
                { 
                    Id = 1, 
                    UsuarioId = 2, 
                    NumeroPedido = "PED-2024-001", 
                    FechaPedido = DateTime.UtcNow.AddDays(-5),
                    Estado = "Completado",
                    Total = 4300000m,
                    Comentarios = "Entrega urgente requerida",
                    DireccionEntrega = "Carrera 45 #78-90, Medellín",
                    FechaCreacion = DateTime.UtcNow.AddDays(-5),
                    Activo = true
                },
                new() 
                { 
                    Id = 2, 
                    UsuarioId = 4, 
                    NumeroPedido = "PED-2024-002", 
                    FechaPedido = DateTime.UtcNow.AddDays(-3),
                    Estado = "Procesando",
                    Total = 1800000m,
                    Comentarios = "Verificar disponibilidad de stock",
                    DireccionEntrega = "Calle 80 #15-20, Barranquilla",
                    FechaCreacion = DateTime.UtcNow.AddDays(-3),
                    Activo = true
                },
                new() 
                { 
                    Id = 3, 
                    UsuarioId = 2, 
                    NumeroPedido = "PED-2024-003", 
                    FechaPedido = DateTime.UtcNow.AddDays(-1),
                    Estado = "Pendiente",
                    Total = 700000m,
                    Comentarios = "Cliente preferencial",
                    DireccionEntrega = "Carrera 45 #78-90, Medellín",
                    FechaCreacion = DateTime.UtcNow.AddDays(-1),
                    Activo = true
                }
            };

            await context.Pedidos.AddRangeAsync(pedidos);
        }

        // Seed PedidoItems
        {
            var pedidoItems = new List<PedidoItem>
            {
                // Pedido 1 - María Elena
                new() { Id = 1, PedidoId = 1, ProductoId = 1, Cantidad = 1, PrecioUnitario = 2500000m, Subtotal = 2500000m, FechaCreacion = DateTime.UtcNow.AddDays(-5), Activo = true },
                new() { Id = 2, PedidoId = 1, ProductoId = 5, Cantidad = 1, PrecioUnitario = 1200000m, Subtotal = 1200000m, FechaCreacion = DateTime.UtcNow.AddDays(-5), Activo = true },
                new() { Id = 3, PedidoId = 1, ProductoId = 6, Cantidad = 1, PrecioUnitario = 350000m, Subtotal = 350000m, FechaCreacion = DateTime.UtcNow.AddDays(-5), Activo = true },
                new() { Id = 4, PedidoId = 1, ProductoId = 7, Cantidad = 1, PrecioUnitario = 180000m, Subtotal = 180000m, FechaCreacion = DateTime.UtcNow.AddDays(-5), Activo = true },
                new() { Id = 5, PedidoId = 1, ProductoId = 4, Cantidad = 1, PrecioUnitario = 450000m, Subtotal = 450000m, FechaCreacion = DateTime.UtcNow.AddDays(-5), Activo = true },
                
                // Pedido 2 - Ana Lucía
                new() { Id = 6, PedidoId = 2, ProductoId = 2, Cantidad = 1, PrecioUnitario = 1800000m, Subtotal = 1800000m, FechaCreacion = DateTime.UtcNow.AddDays(-3), Activo = true },
                
                // Pedido 3 - María Elena
                new() { Id = 7, PedidoId = 3, ProductoId = 6, Cantidad = 1, PrecioUnitario = 350000m, Subtotal = 350000m, FechaCreacion = DateTime.UtcNow.AddDays(-1), Activo = true },
                new() { Id = 8, PedidoId = 3, ProductoId = 7, Cantidad = 1, PrecioUnitario = 180000m, Subtotal = 180000m, FechaCreacion = DateTime.UtcNow.AddDays(-1), Activo = true },
                new() { Id = 9, PedidoId = 3, ProductoId = 9, Cantidad = 1, PrecioUnitario = 420000m, Subtotal = 420000m, FechaCreacion = DateTime.UtcNow.AddDays(-1), Activo = true }
            };

            await context.PedidoItems.AddRangeAsync(pedidoItems);
        }

        await context.SaveChangesAsync();
    }

}
