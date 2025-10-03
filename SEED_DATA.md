# 🌱 Datos Iniciales (Seed Data) - Backend API

## 📋 Resumen

Este documento describe los **datos iniciales** que se cargan automáticamente en la base de datos cuando se inicia la aplicación por primera vez. Los datos incluyen usuarios, roles, productos y pedidos de ejemplo para facilitar las pruebas y demostraciones.

## 🚀 Inicialización Automática

Los datos se cargan automáticamente cuando:
- Se ejecuta la aplicación por primera vez
- La base de datos está vacía
- Se ejecuta `dotnet run --project src/BackendAPI`

## 👥 Usuarios Iniciales

### **Administradores**
| ID | Nombre | Email | Contraseña | Rol |
|----|--------|-------|------------|-----|
| 1 | Santiago Henao Castaño | zsantiagohenao@gmail.com | Admin123! | Administrador |

### **Usuarios Estándar**
| ID | Nombre | Email | Contraseña | Rol |
|----|--------|-------|------------|-----|
| 2 | María Elena García | maria@empresa.com | Maria123! | Usuario |
| 4 | Ana Lucía Martínez | ana@empresa.com | Ana123! | Usuario |

### **Vendedores**
| ID | Nombre | Email | Contraseña | Rol |
|----|--------|-------|------------|-----|
| 3 | Carlos Alberto Rodríguez | carlos@empresa.com | Carlos123! | Vendedor |
| 5 | Pedro José López | pedro@empresa.com | Pedro123! | Vendedor |

## 🏷️ Roles del Sistema

| ID | Nombre | Descripción |
|----|--------|-------------|
| 1 | Administrador | Administrador del sistema |
| 2 | Usuario | Usuario estándar |
| 3 | Vendedor | Vendedor de productos |

## 📦 Productos Iniciales

### **Electrónicos**
| ID | Producto | Precio | Stock | Código |
|----|----------|--------|-------|--------|
| 1 | Laptop Dell Inspiron 15 | $2,500,000 | 15 | DELL-INS15-001 |
| 2 | Smartphone Samsung Galaxy A54 | $1,800,000 | 25 | SAMS-A54-001 |
| 5 | Monitor LG UltraWide 29 | $1,200,000 | 12 | LG-UW29-001 |

### **Muebles**
| ID | Producto | Precio | Stock | Código |
|----|----------|--------|-------|--------|
| 3 | Mesa de Oficina Ejecutiva | $850,000 | 8 | MESA-OF-001 |
| 4 | Silla Ergonómica Profesional | $450,000 | 20 | SILLA-ERG-001 |

### **Accesorios**
| ID | Producto | Precio | Stock | Código |
|----|----------|--------|-------|--------|
| 6 | Teclado Mecánico Logitech | $350,000 | 30 | LOG-GPRO-001 |
| 7 | Mouse Inalámbrico Microsoft | $180,000 | 40 | MS-SURF-001 |

### **Oficina y Redes**
| ID | Producto | Precio | Stock | Código |
|----|----------|--------|-------|--------|
| 8 | Impresora HP LaserJet Pro | $650,000 | 6 | HP-LJ-M404-001 |
| 9 | Router WiFi 6 TP-Link | $420,000 | 10 | TPL-AX73-001 |
| 10 | Disco Duro Externo Seagate 2TB | $280,000 | 18 | SEAG-2TB-001 |

## 🛒 Pedidos de Ejemplo

### **Pedido 1 - María Elena García**
- **Número:** PED-2024-001
- **Estado:** Completado
- **Total:** $4,300,000
- **Productos:**
  - 1x Laptop Dell Inspiron 15 ($2,500,000)
  - 1x Monitor LG UltraWide 29 ($1,200,000)
  - 1x Teclado Mecánico Logitech ($350,000)
  - 1x Mouse Inalámbrico Microsoft ($180,000)
  - 1x Silla Ergonómica Profesional ($450,000)

### **Pedido 2 - Ana Lucía Martínez**
- **Número:** PED-2024-002
- **Estado:** Procesando
- **Total:** $1,800,000
- **Productos:**
  - 1x Smartphone Samsung Galaxy A54 ($1,800,000)

### **Pedido 3 - María Elena García**
- **Número:** PED-2024-003
- **Estado:** Pendiente
- **Total:** $700,000
- **Productos:**
  - 1x Teclado Mecánico Logitech ($350,000)
  - 1x Mouse Inalámbrico Microsoft ($180,000)
  - 1x Router WiFi 6 TP-Link ($420,000)

## 🔐 Credenciales de Acceso

### **Para Testing de la API**

#### **Administrador**
```
Email: zsantiagohenao@gmail.com
Contraseña: Admin123!
```

#### **Usuario Estándar**
```
Email: maria@empresa.com
Contraseña: Maria123!
```

#### **Vendedor**
```
Email: carlos@empresa.com
Contraseña: Carlos123!
```

## 🚀 Cómo Usar los Datos Iniciales

### **1. Iniciar la Aplicación**
```bash
dotnet run --project src/BackendAPI
```

### **2. Verificar Datos Cargados**
```bash
# Verificar usuarios
curl -X GET "https://localhost:5001/api/usuarios" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"

# Verificar productos
curl -X GET "https://localhost:5001/api/productos" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"

# Verificar pedidos
curl -X GET "https://localhost:5001/api/pedidos" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

### **3. Autenticarse con Usuario de Prueba**
```bash
# Login con administrador
curl -X POST "https://localhost:5001/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "zsantiagohenao@gmail.com",
    "password": "Admin123!"
  }'
```

## 🔄 Reinicialización de Datos

### **Eliminar Base de Datos Existente**
```bash
# Eliminar archivo de base de datos SQLite
rm BackendAPIDb.db

# O eliminar base de datos en memoria (desarrollo)
# Los datos se recrearán automáticamente
```

### **Forzar Recarga de Datos**
```bash
# Detener la aplicación y eliminar la base de datos
# Luego reiniciar la aplicación
dotnet run --project src/BackendAPI
```

## 📊 Estadísticas de Datos Iniciales

| Categoría | Cantidad | Descripción |
|-----------|----------|-------------|
| **Usuarios** | 5 | 1 Admin, 2 Usuarios, 2 Vendedores |
| **Roles** | 3 | Administrador, Usuario, Vendedor |
| **Productos** | 10 | Electrónicos, Muebles, Accesorios |
| **Pedidos** | 3 | Estados: Completado, Procesando, Pendiente |
| **Items de Pedido** | 9 | Productos distribuidos en pedidos |

## 🎯 Casos de Uso de Prueba

### **1. Pruebas de Autenticación**
- Login con diferentes roles
- Verificación de permisos
- Renovación de tokens

### **2. Pruebas de Productos**
- Listado con paginación
- Filtros por categoría
- Búsqueda de productos

### **3. Pruebas de Pedidos**
- Creación de nuevos pedidos
- Actualización de estados
- Cálculo de totales

### **4. Pruebas de Usuarios**
- Gestión de usuarios
- Asignación de roles
- Filtros y búsquedas

## 🔧 Personalización de Datos

### **Modificar Datos Iniciales**
Para modificar los datos iniciales, edita el archivo:
```
src/BackendAPI.Infrastructure/Data/SeedData.cs
```

### **Agregar Nuevos Usuarios**
```csharp
new() 
{ 
    Id = 6, 
    Nombre = "Nuevo Usuario", 
    Apellido = "Apellido", 
    Email = "nuevo@empresa.com", 
    // ... otros campos
}
```

### **Agregar Nuevos Productos**
```csharp
new() 
{ 
    Id = 11, 
    Nombre = "Nuevo Producto", 
    Precio = 500000m,
    Stock = 10,
    // ... otros campos
}
```

## 📝 Notas Importantes

1. **Contraseñas:** Todas las contraseñas están hasheadas con SHA256
2. **IDs:** Los IDs están predefinidos para consistencia en las pruebas
3. **Fechas:** Las fechas de creación son UTC para consistencia
4. **Activos:** Todos los registros están marcados como activos por defecto
5. **Relaciones:** Las relaciones entre entidades están correctamente establecidas

## 🚨 Consideraciones de Seguridad

- **Solo para Desarrollo:** Estos datos son únicamente para desarrollo y testing
- **Contraseñas de Prueba:** No usar estas contraseñas en producción
- **Datos Sensibles:** No incluir información real de usuarios o empresas
- **Eliminar en Producción:** Asegurar que el seed data no se ejecute en producción

---

**Los datos iniciales se cargan automáticamente y están listos para usar inmediatamente después de iniciar la aplicación.**
