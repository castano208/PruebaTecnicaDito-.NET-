# 🚀 Pipeline CI/CD - BackendAPI

## 📋 Descripción General

Este pipeline automatiza la construcción, pruebas, análisis de seguridad y despliegue de la API BackendAPI usando GitHub Actions.

## 🔄 Flujo del Pipeline

### **1. Build and Test** (`build-and-test`)
- ✅ **Checkout del código**
- ✅ **Configuración de .NET 8.0**
- ✅ **Cache de paquetes NuGet**
- ✅ **Restauración de dependencias**
- ✅ **Compilación en modo Release**
- ✅ **Ejecución de tests con cobertura**
- ✅ **Publicación de resultados de tests**
- ✅ **Publicación de cobertura de código**

### **2. Security Scan** (`security-scan`)
- 🔒 **Auditoría de vulnerabilidades**
- 🔒 **Verificación de dependencias obsoletas**
- 🔒 **Análisis de seguridad de paquetes**

### **3. Docker Build** (`docker-build`)
- 🐳 **Construcción de imagen Docker**
- 🐳 **Pruebas de la imagen**
- 🐳 **Health check del contenedor**

### **4. Deploy** (`deploy`)
- 🚀 **Publicación de la aplicación**
- 🚀 **Creación de paquete de despliegue**
- 🚀 **Despliegue a Azure Web App**
- 🚀 **Health check post-despliegue**

### **5. Notify** (`notify`)
- 📢 **Notificación del estado del despliegue**

## 🛠️ Configuración Requerida

### **Secrets de GitHub**
```yaml
AZURE_WEBAPP_PUBLISH_PROFILE: # Perfil de publicación de Azure Web App
```

### **Variables de Entorno**
```yaml
DOTNET_VERSION: '8.0.x'
SOLUTION_FILE: 'BackendAPI.sln'
TEST_PROJECT: 'tests/BackendAPI.Tests/BackendAPI.Tests.csproj'
API_PROJECT: 'src/BackendAPI/BackendAPI.csproj'
```

## 📊 Métricas y Reportes

### **Cobertura de Código**
- **Herramienta**: XPlat Code Coverage
- **Formato**: Cobertura Cobertura XML
- **Publicación**: Codecov

### **Resultados de Tests**
- **Herramienta**: xUnit
- **Formato**: TRX
- **Publicación**: GitHub Actions

## 🔧 Comandos Locales

### **Ejecutar Tests**
```bash
dotnet test tests/BackendAPI.Tests/BackendAPI.Tests.csproj --verbosity normal
```

### **Generar Cobertura**
```bash
dotnet test tests/BackendAPI.Tests/BackendAPI.Tests.csproj --collect:"XPlat Code Coverage"
```

### **Compilar en Release**
```bash
dotnet build BackendAPI.sln --configuration Release
```

### **Publicar Aplicación**
```bash
dotnet publish src/BackendAPI/BackendAPI.csproj -c Release -o ./publish
```

## 🐳 Docker

### **Construir Imagen**
```bash
docker build -t backendapi:latest .
```

### **Ejecutar Contenedor**
```bash
docker run -p 8080:80 backendapi:latest
```

### **Health Check**
```bash
curl http://localhost:8080/health
```

## 🚀 Despliegue

### **Azure Web App**
- **Nombre**: `backendapi-prod`
- **URL**: `https://backendapi-prod.azurewebsites.net`
- **Health Check**: `/health`

### **Configuración de Azure**
1. Crear Azure Web App
2. Obtener perfil de publicación
3. Configurar secret `AZURE_WEBAPP_PUBLISH_PROFILE`

## 📈 Monitoreo

### **Health Checks**
- **Endpoint**: `/health`
- **Frecuencia**: Post-despliegue
- **Timeout**: 30 segundos

### **Logs**
- **Ubicación**: `/app/logs`
- **Formato**: Serilog
- **Niveles**: Information, Warning, Error

## 🔄 Triggers

### **Push Events**
- **Branches**: `main`, `develop`
- **Acción**: Build, Test, Security Scan

### **Pull Request Events**
- **Branches**: `main`
- **Acción**: Build, Test, Security Scan

### **Deploy Events**
- **Branch**: `main` únicamente
- **Acción**: Deploy to Production

## 🛡️ Seguridad

### **Vulnerabilidades**
- **Comando**: `dotnet list package --vulnerable --include-transitive`
- **Frecuencia**: Cada push/PR

### **Dependencias Obsoletas**
- **Comando**: `dotnet list package --outdated`
- **Frecuencia**: Cada push/PR

## 📝 Notas Importantes

1. **Cache de NuGet**: Optimiza tiempos de build
2. **Paralelización**: Jobs independientes para mejor rendimiento
3. **Health Checks**: Verificación automática post-despliegue
4. **Rollback**: Manual en caso de fallos
5. **Logs**: Disponibles en Azure Portal

## 🔧 Troubleshooting

### **Build Failures**
```bash
# Verificar dependencias
dotnet restore

# Limpiar cache
dotnet clean
dotnet build
```

### **Test Failures**
```bash
# Ejecutar tests individuales
dotnet test --filter "TestName"

# Ver logs detallados
dotnet test --verbosity detailed
```

### **Deploy Failures**
1. Verificar secretos de Azure
2. Comprobar conectividad
3. Revisar logs de Azure Portal

---

**🎯 Objetivo**: Automatización completa del ciclo de vida de desarrollo con calidad, seguridad y despliegue confiable.
