# Liga Libre API

API REST para la gestión de una liga de fútbol, desarrollada con ASP.NET Core 8.0 siguiendo principios de Clean Architecture.

## 📋 Tabla de Contenidos

- [Características](#-características)
- [Arquitectura](#-arquitectura)
- [Tecnologías y Paquetes NuGet](#-tecnologías-y-paquetes-nuget)
- [Requisitos Previos](#-requisitos-previos)
- [Configuración](#-configuración)
- [Estructura del Proyecto](#-estructura-del-proyecto)
- [Endpoints Principales](#-endpoints-principales)
- [Pruebas Unitarias](#-pruebas-unitarias)
- [Seguridad](#-seguridad)
- [Caché y Mensajería](#-caché-y-mensajería)

## ✨ Características

- ✅ **Autenticación y Autorización** con JWT
- ✅ **CRUD completo** para Clubes, Jugadores, Partidos y Árbitros
- ✅ **Estadísticas de liga** en tiempo real
- ✅ **Validaciones** con FluentValidation
- ✅ **Caché distribuido** con Redis
- ✅ **Mensajería asíncrona** con AWS SQS
- ✅ **Health Checks** para monitoreo
- ✅ **Rate Limiting** para protección de API
- ✅ **Logging estructurado** con middlewares personalizados
- ✅ **Cobertura de pruebas** ~92%

## 🏗️ Arquitectura

El proyecto sigue **Clean Architecture** con separación en capas:

```
LigaLibre/
├── LigaLibre.Domain/          # Entidades y lógica de negocio
├── LigaLibre.Application/     # Casos de uso, DTOs, Interfaces
├── LigaLibre.Infrastructure/  # Implementaciones (DB, Cache, SQS)
├── LigaLibre/                 # API Controllers y Middlewares
└── LigaLibre.Tests/           # Pruebas unitarias
```

## 📦 Tecnologías y Paquetes NuGet

### **Capa API (LigaLibre)**

```xml
<!-- Autenticación y Seguridad -->
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.11" />
<PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="8.0.11" />

<!-- Documentación -->
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.6.2" />

<!-- Health Checks -->
<PackageReference Include="AspNetCore.HealthChecks.Redis" Version="8.0.1" />
<PackageReference Include="AspNetCore.HealthChecks.Aws.Sqs" Version="8.0.1" />
<PackageReference Include="AspNetCore.HealthChecks.SqlServer" Version="8.0.2" />
```

### **Capa Application (LigaLibre.Application)**

```xml
<!-- Validaciones -->
<PackageReference Include="FluentValidation" Version="11.10.0" />
<PackageReference Include="FluentValidation.DependencyInjectionExtensions" Version="11.10.0" />

<!-- Mapeo de objetos -->
<PackageReference Include="Mapster" Version="7.4.0" />
```

### **Capa Infrastructure (LigaLibre.Infrastructure)**

```xml
<!-- Entity Framework Core -->
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.11" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.11" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.11" />

<!-- Redis Cache -->
<PackageReference Include="StackExchange.Redis" Version="2.8.16" />

<!-- AWS SQS -->
<PackageReference Include="AWSSDK.SQS" Version="3.7.400.29" />
<PackageReference Include="AWSSDK.Extensions.NETCore.Setup" Version="3.7.301" />
```

### **Capa Domain (LigaLibre.Domain)**

```xml
<!-- Sin dependencias externas - Solo lógica de negocio pura -->
```

### **Pruebas (LigaLibre.Tests)**

```xml
<!-- Framework de pruebas -->
<PackageReference Include="xunit" Version="2.9.2" />
<PackageReference Include="xunit.runner.visualstudio" Version="2.8.2" />
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.11.1" />

<!-- Mocking -->
<PackageReference Include="Moq" Version="4.20.72" />

<!-- Testing de validaciones -->
<PackageReference Include="FluentValidation.TestHelper" Version="11.10.0" />

<!-- Testing de EF Core -->
<PackageReference Include="Microsoft.EntityFrameworkCore.InMemory" Version="8.0.11" />
```

## 🔧 Requisitos Previos

- **.NET 8.0 SDK** o superior
- **Docker Desktop** (recomendado para desarrollo local)
- **Visual Studio 2022** o **VS Code**

### Opción 1: Usando Docker (Recomendado)
- Docker Desktop instalado y corriendo
- Ver [DOCKER.md](DOCKER.md) para instrucciones detalladas

### Opción 2: Instalación Manual
- **SQL Server** (LocalDB o instancia completa)
- **Redis** (local o remoto)
- **AWS Account** (para SQS) o LocalStack

## ⚙️ Configuración

### 1. Clonar el repositorio

```bash
git clone https://github.com/roldanValentin-dev/Liga-Libre-Api.git
cd Liga-Libre-Api
```

### 2. Iniciar servicios con Docker (Recomendado)

```bash
# Iniciar SQL Server, Redis y LocalStack
docker-compose up -d

# Verificar que los servicios estén corriendo
docker-compose ps
```

Para más detalles sobre Docker, consulta [DOCKER.md](DOCKER.md)

### 3. Configurar appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=LigaLibreDb;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=true"
  },
  "Jwt": {
    "Key": "TU_CLAVE_SECRETA_MUY_SEGURA_DE_AL_MENOS_32_CARACTERES",
    "Issuer": "LigaLibreAPI",
    "Audience": "LigaLibreClient",
    "ExpireMinutes": 60
  },
  "Redis": {
    "Configuration": "localhost:6379"
  },
  "AWS": {
    "Region": "us-east-1",
    "ServiceURL": "http://localhost:4566",
    "SQS": {
      "ClubEventQueue": "club-events",
      "PlayerEventQueue": "player-events",
      "MatchEventQueue": "match-events",
      "RefereeEventQueue": "referee-events"
    }
  }
}
```

### Configuración con Docker

Si usas Docker (recomendado), los servicios estarán disponibles en:
- **SQL Server**: `localhost:1433` (usuario: `sa`, password: `YourStrong@Passw0rd`)
- **Redis**: `localhost:6379`
- **LocalStack (SQS)**: `http://localhost:4566`

Ver [DOCKER.md](DOCKER.md) para más detalles.
}
```

### 4. Aplicar migraciones

```bash
cd LigaLibre
dotnet ef database update
```

### 5. Ejecutar el proyecto

```bash
dotnet run --project LigaLibre
```

La API estará disponible en: `https://localhost:7001`

## 📁 Estructura del Proyecto

```
LigaLibre/
│
├── LigaLibre.Domain/
│   ├── Entities/              # Club, Player, Match, Referee, User
│   ├── Enums/                 # MatchStatus, RefereeCategory
│   └── Interfaces/            # Contratos de repositorios
│
├── LigaLibre.Application/
│   ├── DTOs/                  # Data Transfer Objects
│   ├── Interfaces/            # Contratos de servicios
│   ├── Services/              # Lógica de negocio
│   ├── Validators/            # FluentValidation validators
│   ├── Mapping/               # Configuración de Mapster
│   └── DependencyInjections.cs
│
├── LigaLibre.Infrastructure/
│   ├── Data/                  # DbContext y configuraciones
│   ├── Repositories/          # Implementación de repositorios
│   ├── Services/              # Redis, SQS, Auth
│   └── DependencyInjections.cs
│
├── LigaLibre/
│   ├── Controllers/           # API Endpoints
│   ├── Middlewares/           # Logging, Rate Limiting, Error Handling
│   └── Program.cs
│
└── LigaLibre.Tests/
    ├── Controllers/           # Tests de controladores
    ├── Services/              # Tests de servicios
    ├── Repositories/          # Tests de repositorios
    ├── Validators/            # Tests de validadores
    ├── DTOs/                  # Tests de DTOs
    ├── Entities/              # Tests de entidades
    └── Middlewares/           # Tests de middlewares
```

## 🌐 Endpoints Principales

### **Autenticación**

```http
POST /api/Auth/register          # Registrar usuario
POST /api/Auth/login             # Iniciar sesión
```

### **Clubes**

```http
GET    /api/Club/GetAllClubs     # Obtener todos los clubes
GET    /api/Club/GetById/{id}    # Obtener club por ID
POST   /api/Club/CreateClub      # Crear club
PUT    /api/Club/UpdateClub      # Actualizar club
DELETE /api/Club/DeleteClub      # Eliminar club
```

### **Jugadores**

```http
GET    /api/Player/GetAllPlayers           # Obtener todos los jugadores
GET    /api/Player/GetPlayersByClub        # Obtener jugadores por club
GET    /api/Player/GetPlayerById           # Obtener jugador por ID
POST   /api/Player/CreatePlayer            # Crear jugador
PUT    /api/Player/UpdatePlayer            # Actualizar jugador
DELETE /api/Player/DeletePlayer            # Eliminar jugador
```

### **Partidos**

```http
GET    /api/Match/GetAllMatches            # Obtener todos los partidos
GET    /api/Match/GetMatchById             # Obtener partido por ID
GET    /api/Match/GetMatchesByClub         # Obtener partidos por club
GET    /api/Match/GetMatchesByRound        # Obtener partidos por jornada
GET    /api/Match/GetMatchesByStatus       # Obtener partidos por estado
POST   /api/Match/CreateMatch              # Crear partido
PUT    /api/Match/UpdateMatch              # Actualizar partido
DELETE /api/Match/DeleteMatch              # Eliminar partido
```

### **Árbitros**

```http
GET    /api/Referee/GetAllReferees         # Obtener todos los árbitros
GET    /api/Referee/GetActivesReferees     # Obtener árbitros activos
GET    /api/Referee/GetRefereesById        # Obtener árbitro por ID
POST   /api/Referee/CreateReferee          # Crear árbitro
PUT    /api/Referee/UpdateReferee          # Actualizar árbitro
DELETE /api/Referee/DeleteReferee          # Eliminar árbitro
```

### **Estadísticas**

```http
GET /api/Statistics/league                 # Estadísticas generales de la liga
GET /api/Statistics/matches                # Estadísticas de partidos
GET /api/Statistics/players                # Estadísticas de jugadores
```

### **Health Check**

```http
GET /api/Health                            # Estado de salud del sistema
```

## 🧪 Pruebas Unitarias

El proyecto cuenta con **~92% de cobertura** de pruebas unitarias.

### Ejecutar todas las pruebas

```bash
dotnet test
```

### Ejecutar con cobertura

```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Estructura de pruebas

- **48 archivos de pruebas**
- **Controllers**: 100% cobertura (7/7)
- **Services**: 100% cobertura (6/6)
- **Repositories**: 100% cobertura (4/4)
- **Validators**: 100% cobertura (8/8)
- **DTOs**: 100% cobertura
- **Entities**: 100% cobertura (4/4)
- **Middlewares**: 100% cobertura (3/3)

## 🔒 Seguridad

### Autenticación JWT

- Tokens con expiración configurable
- Roles: `User`, `Admin`
- Protección de endpoints con `[Authorize]`

### Rate Limiting

- Límite de 100 requests por minuto por IP
- Configurable en `RateLimitingMiddleware`

### Validaciones

- FluentValidation en todos los DTOs
- Validación de datos de entrada
- Mensajes de error en español

## 🚀 Caché y Mensajería

### Redis Cache

- Caché de consultas frecuentes
- TTL configurable (10 minutos por defecto)
- Invalidación automática en operaciones CUD

### AWS SQS

- Eventos asíncronos para:
  - Creación/Actualización/Eliminación de Clubes
  - Creación/Actualización/Eliminación de Jugadores
  - Creación/Actualización/Eliminación de Partidos
  - Creación/Actualización/Eliminación de Árbitros

## 📊 Monitoreo

### Health Checks

Verifica el estado de:
- ✅ Base de datos SQL Server
- ✅ Redis Cache
- ✅ AWS SQS

Endpoint: `GET /api/Health`

Respuesta:
```json
{
  "status": "Healthy",
  "totalDuration": 150.5,
  "checks": [
    {
      "name": "Database",
      "status": "Healthy",
      "duration": 50.2
    },
    {
      "name": "Redis",
      "status": "Healthy",
      "duration": 30.1
    },
    {
      "name": "SQS",
      "status": "Healthy",
      "duration": 70.2
    }
  ]
}
```

## 📝 Convenciones de Código

- **Idioma**: Español para comentarios y mensajes
- **Documentación XML**: Todos los métodos públicos
- **Naming**: PascalCase para clases y métodos, camelCase para variables
- **Async/Await**: Todos los métodos de I/O son asíncronos

## 🤝 Contribuciones

Este proyecto fue desarrollado como parte de un curso académico.

## 📄 Licencia

Este proyecto es de uso educativo.

## 👨‍💻 Autor

**Valentín Roldán**
- GitHub: [@roldanValentin-dev](https://github.com/roldanValentin-dev)
- Repositorio: [Liga-Libre-Api](https://github.com/roldanValentin-dev/Liga-Libre-Api)

---

⭐ Si este proyecto te fue útil, considera darle una estrella en GitHub
