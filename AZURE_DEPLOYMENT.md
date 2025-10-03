# 🚀 Configuración de Despliegue en Azure

## 📋 Prerrequisitos

### **1. Crear Azure Web App**

```bash
# Crear Resource Group
az group create --name BackendAPI-RG --location "East US"

# Crear App Service Plan
az appservice plan create --name BackendAPI-Plan --resource-group BackendAPI-RG --sku B1 --is-linux

# Crear Web App
az webapp create --resource-group BackendAPI-RG --plan BackendAPI-Plan --name backendapi-prod --runtime "DOTNET|8.0"
```

### **2. Configurar Secrets en GitHub**

#### **Opción A: Azure Credentials (Recomendado)**

1. **Crear Service Principal:**
```bash
az ad sp create-for-rbac --name "BackendAPI-GitHub" --role contributor --scopes /subscriptions/{subscription-id}/resourceGroups/BackendAPI-RG --sdk-auth
```

2. **Copiar el JSON resultante y agregarlo como secret:**
   - **Nombre**: `AZURE_CREDENTIALS`
   - **Valor**: El JSON completo del comando anterior

#### **Opción B: Publish Profile (Alternativo)**

1. **Descargar Publish Profile:**
```bash
az webapp deployment list-publishing-profiles --name backendapi-prod --resource-group BackendAPI-RG --xml
```

2. **Agregar como secret:**
   - **Nombre**: `AZURE_WEBAPP_PUBLISH_PROFILE`
   - **Valor**: El contenido XML del publish profile

## 🔧 Configuración de GitHub Secrets

### **Secrets Requeridos**

| Secret | Descripción | Ejemplo |
|--------|-------------|---------|
| `AZURE_CREDENTIALS` | Service Principal JSON | `{"clientId":"...","clientSecret":"..."}` |
| `AZURE_WEBAPP_PUBLISH_PROFILE` | Publish Profile XML | `<publishData>...</publishData>` |

### **Cómo Agregar Secrets en GitHub**

1. **Ir a Settings** → **Secrets and variables** → **Actions**
2. **Click "New repository secret"**
3. **Agregar cada secret con su valor**

## 🚀 Configuración de la Web App

### **Variables de Entorno**

```bash
# Configurar variables de entorno en Azure
az webapp config appsettings set --name backendapi-prod --resource-group BackendAPI-RG --settings \
  ASPNETCORE_ENVIRONMENT=Production \
  ConnectionStrings__DefaultConnection="Server=..." \
  Jwt__Key="your-jwt-secret-key" \
  Jwt__Issuer="BackendAPI" \
  Jwt__Audience="BackendAPI_Users"
```

### **Configuración de Logs**

```bash
# Habilitar Application Insights
az webapp config appsettings set --name backendapi-prod --resource-group BackendAPI-RG --settings \
  APPINSIGHTS_INSTRUMENTATIONKEY="your-instrumentation-key"

# Configurar logging
az webapp log config --name backendapi-prod --resource-group BackendAPI-RG --application-logging true --level information
```

## 🔍 Verificación del Despliegue

### **Health Check**

```bash
# Verificar que la API responde
curl https://backendapi-prod.azurewebsites.net/health

# Verificar Swagger
curl https://backendapi-prod.azurewebsites.net/swagger
```

### **Logs de la Aplicación**

```bash
# Ver logs en tiempo real
az webapp log tail --name backendapi-prod --resource-group BackendAPI-RG

# Descargar logs
az webapp log download --name backendapi-prod --resource-group BackendAPI-RG
```

## 🛠️ Troubleshooting

### **Error: "No credentials found"**

**Solución:**
1. Verificar que `AZURE_CREDENTIALS` está configurado
2. Verificar que el JSON del service principal es válido
3. Verificar que el service principal tiene permisos en el resource group

### **Error: "Deployment Failed"**

**Solución:**
1. Verificar que la Web App existe
2. Verificar que el runtime es correcto (.NET 8.0)
3. Verificar que el package se generó correctamente

### **Error: "Health check failed"**

**Solución:**
1. Verificar que la aplicación inicia correctamente
2. Verificar que el endpoint `/health` está implementado
3. Verificar logs de la aplicación en Azure Portal

## 📊 Monitoreo

### **Application Insights**

```bash
# Crear Application Insights
az monitor app-insights component create --app backendapi-insights --location "East US" --resource-group BackendAPI-RG

# Obtener instrumentation key
az monitor app-insights component show --app backendapi-insights --resource-group BackendAPI-RG --query instrumentationKey
```

### **Alertas**

```bash
# Crear alerta de disponibilidad
az monitor metrics alert create --name "BackendAPI-Availability" --resource-group BackendAPI-RG --scopes /subscriptions/{subscription-id}/resourceGroups/BackendAPI-RG/providers/Microsoft.Web/sites/backendapi-prod --condition "avg availabilityResults/availabilityPercentage < 99" --description "Alert when availability drops below 99%"
```

## 🔐 Seguridad

### **HTTPS y Dominio Personalizado**

```bash
# Configurar dominio personalizado
az webapp config hostname add --webapp-name backendapi-prod --resource-group BackendAPI-RG --hostname api.tudominio.com

# Configurar SSL
az webapp config ssl bind --name backendapi-prod --resource-group BackendAPI-RG --certificate-thumbprint {thumbprint}
```

### **Firewall y Acceso**

```bash
# Configurar IP restrictions
az webapp config access-restriction add --name backendapi-prod --resource-group BackendAPI-RG --rule-name "Allow GitHub Actions" --action Allow --ip-address 0.0.0.0/0
```

---

**🎯 Objetivo**: Despliegue automatizado y confiable de BackendAPI en Azure con monitoreo y seguridad adecuados.
