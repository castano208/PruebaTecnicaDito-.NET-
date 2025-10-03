# 🚀 Ejemplos de Uso de la API - Backend API

## 📋 Resumen

Esta guía proporciona ejemplos prácticos de cómo usar la API Backend con diferentes herramientas y métodos.

## 🔐 Autenticación

### **Login con cURL**
```bash
curl -X POST "https://localhost:5001/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "zsantiagohenao@gmail.com",
    "password": "Admin123!"
  }'
```

### **Login con PowerShell**
```powershell
$body = @{
    email = "zsantiagohenao@gmail.com"
    password = "Admin123!"
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:5001/api/auth/login" `
  -Method POST `
  -ContentType "application/json" `
  -Body $body
```

### **Login con JavaScript (Fetch)**
```javascript
const response = await fetch('https://localhost:5001/api/auth/login', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json',
  },
  body: JSON.stringify({
    email: 'zsantiagohenao@gmail.com',
    password: 'Admin123!'
  })
});

const data = await response.json();
console.log(data.token); // Guardar este token
```

## 🔑 Uso del Token JWT

### **Guardar el Token**
```bash
# Ejemplo de respuesta del login
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "refresh_token_here",
  "expires": "2024-01-01T12:00:00Z",
  "usuario": {
    "id": 1,
    "nombre": "Santiago",
    "apellido": "Henao Castaño",
    "email": "zsantiagohenao@gmail.com"
  }
}
```

### **Usar el Token en Requests**
```bash
# Reemplazar YOUR_JWT_TOKEN con el token obtenido
TOKEN="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."

# Ejemplo: Obtener usuarios
curl -X GET "https://localhost:5001/api/usuarios" \
  -H "Authorization: Bearer $TOKEN"
```

## 👥 Gestión de Usuarios

### **Obtener Todos los Usuarios**
```bash
curl -X GET "https://localhost:5001/api/usuarios" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

### **Obtener Usuario por ID**
```bash
curl -X GET "https://localhost:5001/api/usuarios/1" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

### **Crear Nuevo Usuario**
```bash
curl -X POST "https://localhost:5001/api/usuarios" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "nombre": "Nuevo Usuario",
    "apellido": "Apellido",
    "email": "nuevo@ejemplo.com",
    "telefono": "3001234567",
    "fechaNacimiento": "1990-01-01"
  }'
```

## 📦 Gestión de Productos

### **Obtener Todos los Productos**
```bash
curl -X GET "https://localhost:5001/api/productos" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

### **Obtener Producto por ID**
```bash
curl -X GET "https://localhost:5001/api/productos/1" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

### **Crear Nuevo Producto**
```bash
curl -X POST "https://localhost:5001/api/productos" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "nombre": "Nuevo Producto",
    "descripcion": "Descripción del producto",
    "precio": 100000,
    "stock": 10,
    "categoria": "Electrónicos",
    "codigo": "PROD-001"
  }'
```

## 🛒 Gestión de Pedidos

### **Obtener Todos los Pedidos**
```bash
curl -X GET "https://localhost:5001/api/pedidos" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

### **Obtener Pedido por ID**
```bash
curl -X GET "https://localhost:5001/api/pedidos/1" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

### **Crear Nuevo Pedido**
```bash
curl -X POST "https://localhost:5001/api/pedidos" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "usuarioId": 1,
    "comentarios": "Pedido de prueba",
    "direccionEntrega": "Calle 123 #45-67",
    "pedidoItems": [
      {
        "productoId": 1,
        "cantidad": 2
      },
      {
        "productoId": 2,
        "cantidad": 1
      }
    ]
  }'
```

## 🔄 Operaciones de Autenticación

### **Renovar Token**
```bash
curl -X POST "https://localhost:5001/api/auth/refresh" \
  -H "Content-Type: application/json" \
  -d '{
    "refreshToken": "YOUR_REFRESH_TOKEN"
  }'
```

### **Cerrar Sesión**
```bash
curl -X POST "https://localhost:5001/api/auth/logout" \
  -H "Content-Type: application/json" \
  -d '{
    "refreshToken": "YOUR_REFRESH_TOKEN"
  }'
```

### **Registrar Nuevo Usuario**
```bash
curl -X POST "https://localhost:5001/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{
    "nombre": "Usuario Nuevo",
    "apellido": "Apellido",
    "email": "usuario@ejemplo.com",
    "password": "Password123!",
    "telefono": "3001234567",
    "fechaNacimiento": "1990-01-01"
  }'
```

## 🏥 Endpoints de Sistema

### **Health Check**
```bash
curl -X GET "https://localhost:5001/health"
```

### **Swagger UI**
```
https://localhost:5001/swagger
```

## 🚨 Manejo de Errores

### **Error 415: Unsupported Media Type**
```bash
# ❌ Incorrecto (sin Content-Type)
curl -X POST "https://localhost:5001/api/auth/login" \
  -d '{"email": "zsantiagohenao@gmail.com", "password": "Admin123!"}'

# ✅ Correcto (con Content-Type)
curl -X POST "https://localhost:5001/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email": "zsantiagohenao@gmail.com", "password": "Admin123!"}'
```

### **Error 401: Unauthorized**
```bash
# ❌ Sin token
curl -X GET "https://localhost:5001/api/usuarios"

# ✅ Con token
curl -X GET "https://localhost:5001/api/usuarios" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

### **Error 400: Bad Request**
```bash
# ❌ JSON malformado
curl -X POST "https://localhost:5001/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email": "zsantiagohenao@gmail.com"}'  # Falta password

# ✅ JSON correcto
curl -X POST "https://localhost:5001/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email": "zsantiagohenao@gmail.com", "password": "Admin123!"}'
```

## 🧪 Testing con Postman

### **Configuración de Postman**

1. **Crear nueva colección:** "Backend API"
2. **Configurar variables:**
   - `baseUrl`: `https://localhost:5001`
   - `token`: (se llenará automáticamente)

3. **Request de Login:**
   - Method: `POST`
   - URL: `{{baseUrl}}/api/auth/login`
   - Headers: `Content-Type: application/json`
   - Body (raw JSON):
   ```json
   {
     "email": "zsantiagohenao@gmail.com",
     "password": "Admin123!"
   }
   ```

4. **Script de Post-Test (para guardar token):**
   ```javascript
   if (pm.response.code === 200) {
     const response = pm.response.json();
     pm.collectionVariables.set("token", response.token);
   }
   ```

5. **Request con Token:**
   - Method: `GET`
   - URL: `{{baseUrl}}/api/usuarios`
   - Headers: `Authorization: Bearer {{token}}`

## 🔧 Scripts de Automatización

### **Script de Login (Bash)**
```bash
#!/bin/bash

# Función para hacer login y obtener token
login() {
  local response=$(curl -s -X POST "https://localhost:5001/api/auth/login" \
    -H "Content-Type: application/json" \
    -d '{
      "email": "zsantiagohenao@gmail.com",
      "password": "Admin123!"
    }')
  
  echo $response | jq -r '.token'
}

# Usar la función
TOKEN=$(login)
echo "Token obtenido: $TOKEN"

# Usar el token
curl -X GET "https://localhost:5001/api/usuarios" \
  -H "Authorization: Bearer $TOKEN"
```

### **Script de Login (PowerShell)**
```powershell
# Función para hacer login
function Login-API {
  $body = @{
    email = "zsantiagohenao@gmail.com"
    password = "Admin123!"
  } | ConvertTo-Json

  $response = Invoke-RestMethod -Uri "https://localhost:5001/api/auth/login" `
    -Method POST `
    -ContentType "application/json" `
    -Body $body

  return $response.token
}

# Usar la función
$token = Login-API
Write-Host "Token obtenido: $token"

# Usar el token
$headers = @{
  "Authorization" = "Bearer $token"
}

$usuarios = Invoke-RestMethod -Uri "https://localhost:5001/api/usuarios" `
  -Method GET `
  -Headers $headers

$usuarios | ConvertTo-Json
```

## 📊 Monitoreo y Debugging

### **Verificar Estado de la API**
```bash
# Health check
curl -X GET "https://localhost:5001/health"

# Verificar que la API esté funcionando
curl -X GET "https://localhost:5001/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{}'  # Debería devolver error 400, no 500
```

### **Logs de la Aplicación**
```bash
# Ver logs en tiempo real (si está configurado)
tail -f logs/backendapi-*.txt
```
