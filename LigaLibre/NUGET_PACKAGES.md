# 📦 Paquetes NuGet Esenciales para Proyectos ASP.NET Core

Guía de referencia de paquetes NuGet utilizados en Liga Libre API, útil para futuros proyectos.

## 🔐 Autenticación y Seguridad

### JWT Authentication
```bash
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.11
```
**Uso**: Autenticación basada en tokens JWT

### Identity Framework
```bash
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.11
```
**Uso**: Gestión de usuarios, roles y autenticación

---

## 🗄️ Base de Datos (Entity Framework Core)

### SQL Server Provider
```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.11
```
**Uso**: Proveedor de SQL Server para EF Core

### EF Core Tools
```bash
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.11
```
**Uso**: Migraciones y comandos de CLI

### EF Core Base
```bash
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.11
```
**Uso**: ORM principal

### In-Memory Database (Testing)
```bash
dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 8.0.11
```
**Uso**: Base de datos en memoria para pruebas unitarias

---

## ✅ Validaciones

### FluentValidation
```bash
dotnet add package FluentValidation --version 11.10.0
```
**Uso**: Validaciones fluidas y expresivas

### FluentValidation DI
```bash
dotnet add package FluentValidation.DependencyInjectionExtensions --version 11.10.0
```
**Uso**: Integración con inyección de dependencias

### FluentValidation Testing
```bash
dotnet add package FluentValidation.TestHelper --version 11.10.0
```
**Uso**: Helpers para probar validadores

---

## 🔄 Mapeo de Objetos

### Mapster
```bash
dotnet add package Mapster --version 7.4.0
```
**Uso**: Mapeo rápido entre DTOs y entidades
**Alternativa**: AutoMapper

---

## 🚀 Caché Distribuido

### Redis
```bash
dotnet add package StackExchange.Redis --version 2.8.16
```
**Uso**: Cliente de Redis para caché distribuido

---

## ☁️ AWS Services

### AWS SQS
```bash
dotnet add package AWSSDK.SQS --version 3.7.400.29
```
**Uso**: Mensajería con AWS Simple Queue Service

### AWS Extensions
```bash
dotnet add package AWSSDK.Extensions.NETCore.Setup --version 3.7.301
```
**Uso**: Configuración de servicios AWS en .NET Core

---

## 📊 Health Checks

### SQL Server Health Check
```bash
dotnet add package AspNetCore.HealthChecks.SqlServer --version 8.0.2
```
**Uso**: Verificar estado de SQL Server

### Redis Health Check
```bash
dotnet add package AspNetCore.HealthChecks.Redis --version 8.0.1
```
**Uso**: Verificar estado de Redis

### AWS SQS Health Check
```bash
dotnet add package AspNetCore.HealthChecks.Aws.Sqs --version 8.0.1
```
**Uso**: Verificar estado de AWS SQS

---

## 📖 Documentación API

### Swagger/OpenAPI
```bash
dotnet add package Swashbuckle.AspNetCore --version 6.6.2
```
**Uso**: Documentación interactiva de API

---

## 🧪 Testing

### xUnit Framework
```bash
dotnet add package xunit --version 2.9.2
dotnet add package xunit.runner.visualstudio --version 2.8.2
dotnet add package Microsoft.NET.Test.Sdk --version 17.11.1
```
**Uso**: Framework de pruebas unitarias

### Moq
```bash
dotnet add package Moq --version 4.20.72
```
**Uso**: Librería de mocking para pruebas

---

## 📝 Logging

### Serilog (Opcional - No usado en este proyecto)
```bash
dotnet add package Serilog.AspNetCore --version 8.0.0
dotnet add package Serilog.Sinks.Console --version 5.0.0
dotnet add package Serilog.Sinks.File --version 5.0.0
```
**Uso**: Logging estructurado avanzado

---

## 🔧 Utilidades Adicionales

### Newtonsoft.Json
```bash
dotnet add package Newtonsoft.Json --version 13.0.3
```
**Uso**: Serialización/deserialización JSON (alternativa a System.Text.Json)

### Dapper (Opcional)
```bash
dotnet add package Dapper --version 2.1.28
```
**Uso**: Micro-ORM para consultas SQL optimizadas

### MediatR (Opcional)
```bash
dotnet add package MediatR --version 12.2.0
```
**Uso**: Patrón Mediator para CQRS

---

## 🌐 CORS y HTTP

### CORS (Incluido en ASP.NET Core)
```csharp
// En Program.cs
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
```

---

## 📦 Instalación Completa para Nuevo Proyecto

### Proyecto API
```bash
# Crear proyecto
dotnet new webapi -n MiProyecto.API

# Autenticación
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore

# Base de datos
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools

# Validaciones
dotnet add package FluentValidation
dotnet add package FluentValidation.DependencyInjectionExtensions

# Mapeo
dotnet add package Mapster

# Documentación
dotnet add package Swashbuckle.AspNetCore

# Health Checks
dotnet add package AspNetCore.HealthChecks.SqlServer
```

### Proyecto de Pruebas
```bash
# Crear proyecto de pruebas
dotnet new xunit -n MiProyecto.Tests

# Testing
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package Moq
dotnet add package FluentValidation.TestHelper
dotnet add package Microsoft.EntityFrameworkCore.InMemory
```

---

## 🎯 Paquetes por Escenario

### API REST Básica
- ✅ Microsoft.EntityFrameworkCore.SqlServer
- ✅ Swashbuckle.AspNetCore
- ✅ FluentValidation

### API con Autenticación
- ✅ Microsoft.AspNetCore.Authentication.JwtBearer
- ✅ Microsoft.AspNetCore.Identity.EntityFrameworkCore

### API con Caché
- ✅ StackExchange.Redis

### API con Mensajería
- ✅ AWSSDK.SQS (AWS)
- ✅ RabbitMQ.Client (RabbitMQ)
- ✅ Azure.Messaging.ServiceBus (Azure)

### API con Monitoreo
- ✅ AspNetCore.HealthChecks.*
- ✅ Serilog.AspNetCore

---

## 📚 Recursos Adicionales

### Documentación Oficial
- [NuGet Gallery](https://www.nuget.org/)
- [Microsoft Docs - ASP.NET Core](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)

### Herramientas
- **NuGet Package Explorer**: Explorar paquetes
- **dotnet outdated**: Verificar paquetes desactualizados
  ```bash
  dotnet tool install --global dotnet-outdated-tool
  dotnet outdated
  ```

---

## ⚠️ Notas Importantes

1. **Versiones**: Mantén las versiones consistentes entre proyectos de la misma solución
2. **Seguridad**: Actualiza regularmente los paquetes para parches de seguridad
3. **Compatibilidad**: Verifica compatibilidad con tu versión de .NET
4. **Licencias**: Revisa las licencias de los paquetes antes de usar en producción

---

## 🔄 Comandos Útiles

```bash
# Listar paquetes instalados
dotnet list package

# Actualizar paquete específico
dotnet add package NombrePaquete --version X.X.X

# Remover paquete
dotnet remove package NombrePaquete

# Restaurar paquetes
dotnet restore

# Limpiar y restaurar
dotnet clean && dotnet restore
```

---

**Última actualización**: Diciembre 2024  
**Versión .NET**: 8.0
