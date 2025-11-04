# 🐳 Configuración Docker para Liga Libre API

Guía para ejecutar los servicios de infraestructura usando Docker.

## 📋 Servicios Incluidos

- **SQL Server 2022**: Base de datos principal
- **Redis 7**: Caché distribuido
- **LocalStack**: Emulador de AWS SQS para desarrollo local

## 🚀 Inicio Rápido

### 1. Requisitos Previos

- [Docker Desktop](https://www.docker.com/products/docker-desktop) instalado
- Al menos 4GB de RAM disponible para Docker

### 2. Iniciar todos los servicios

```bash
# Desde la raíz del proyecto
docker-compose up -d
```

### 3. Verificar que los servicios estén corriendo

```bash
docker-compose ps
```

Deberías ver:
```
NAME                    STATUS    PORTS
ligalibre-sqlserver     Up        0.0.0.0:1433->1433/tcp
ligalibre-redis         Up        0.0.0.0:6379->6379/tcp
ligalibre-localstack    Up        0.0.0.0:4566->4566/tcp
```

## 🔧 Configuración de la API

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=LigaLibreDb;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=true"
  },
  "Redis": {
    "Configuration": "localhost:6379"
  },
  "AWS": {
    "Region": "us-east-1",
    "Profile": "localstack",
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

## 📦 Servicios Individuales

### SQL Server

#### Iniciar solo SQL Server
```bash
docker-compose up -d sqlserver
```

#### Conectarse a SQL Server
- **Host**: localhost
- **Puerto**: 1433
- **Usuario**: sa
- **Contraseña**: YourStrong@Passw0rd

#### Ejecutar migraciones
```bash
cd LigaLibre
dotnet ef database update
```

#### Conectar con SQL Server Management Studio (SSMS)
```
Server: localhost,1433
Authentication: SQL Server Authentication
Login: sa
Password: YourStrong@Passw0rd
```

---

### Redis

#### Iniciar solo Redis
```bash
docker-compose up -d redis
```

#### Conectarse a Redis CLI
```bash
docker exec -it ligalibre-redis redis-cli
```

#### Comandos útiles de Redis
```bash
# Ver todas las keys
KEYS *

# Ver valor de una key
GET "key_name"

# Limpiar toda la caché
FLUSHALL

# Ver información del servidor
INFO
```

#### Herramientas GUI para Redis
- [RedisInsight](https://redis.com/redis-enterprise/redis-insight/)
- [Another Redis Desktop Manager](https://github.com/qishibo/AnotherRedisDesktopManager)

---

### LocalStack (AWS SQS)

#### Iniciar solo LocalStack
```bash
docker-compose up -d localstack
```

#### Crear colas SQS
```bash
# Instalar AWS CLI si no lo tienes
# Windows: choco install awscli
# Mac: brew install awscli
# Linux: apt-get install awscli

# Configurar AWS CLI para LocalStack
aws configure --profile localstack
# AWS Access Key ID: test
# AWS Secret Access Key: test
# Default region name: us-east-1
# Default output format: json

# Crear las colas
aws --endpoint-url=http://localhost:4566 sqs create-queue --queue-name club-events --profile localstack
aws --endpoint-url=http://localhost:4566 sqs create-queue --queue-name player-events --profile localstack
aws --endpoint-url=http://localhost:4566 sqs create-queue --queue-name match-events --profile localstack
aws --endpoint-url=http://localhost:4566 sqs create-queue --queue-name referee-events --profile localstack
```

#### Listar colas
```bash
aws --endpoint-url=http://localhost:4566 sqs list-queues --profile localstack
```

#### Ver mensajes en una cola
```bash
aws --endpoint-url=http://localhost:4566 sqs receive-message --queue-url http://localhost:4566/000000000000/club-events --profile localstack
```

---

## 🛠️ Comandos Útiles

### Ver logs de los servicios
```bash
# Todos los servicios
docker-compose logs -f

# Servicio específico
docker-compose logs -f sqlserver
docker-compose logs -f redis
docker-compose logs -f localstack
```

### Detener servicios
```bash
# Detener todos
docker-compose stop

# Detener servicio específico
docker-compose stop sqlserver
```

### Reiniciar servicios
```bash
# Reiniciar todos
docker-compose restart

# Reiniciar servicio específico
docker-compose restart redis
```

### Eliminar servicios y volúmenes
```bash
# Detener y eliminar contenedores
docker-compose down

# Eliminar también los volúmenes (CUIDADO: borra los datos)
docker-compose down -v
```

### Ver uso de recursos
```bash
docker stats
```

---

## 🔍 Troubleshooting

### SQL Server no inicia

**Problema**: El contenedor se detiene inmediatamente

**Solución**:
```bash
# Ver logs
docker-compose logs sqlserver

# Verificar que tienes suficiente RAM (mínimo 2GB)
# Verificar que el puerto 1433 no esté en uso
netstat -ano | findstr :1433
```

### Redis no conecta

**Problema**: Error de conexión a Redis

**Solución**:
```bash
# Verificar que Redis está corriendo
docker-compose ps redis

# Probar conexión
docker exec -it ligalibre-redis redis-cli ping
# Debería responder: PONG
```

### LocalStack no responde

**Problema**: No se pueden crear colas o enviar mensajes

**Solución**:
```bash
# Verificar logs
docker-compose logs localstack

# Verificar que el servicio está listo
curl http://localhost:4566/_localstack/health

# Reiniciar LocalStack
docker-compose restart localstack
```

### Puerto ya en uso

**Problema**: Error "port is already allocated"

**Solución**:
```bash
# Windows: Ver qué proceso usa el puerto
netstat -ano | findstr :1433

# Matar el proceso (reemplaza PID)
taskkill /PID <PID> /F

# O cambiar el puerto en docker-compose.yml
ports:
  - "1434:1433"  # Usar puerto 1434 en lugar de 1433
```

---

## 🔐 Seguridad

### ⚠️ IMPORTANTE para Producción

Los valores por defecto son **SOLO para desarrollo local**:

```yaml
# ❌ NO usar en producción
SA_PASSWORD=YourStrong@Passw0rd

# ✅ Usar variables de entorno
SA_PASSWORD=${SQL_PASSWORD}
```

### Usar archivo .env

Crear archivo `.env` en la raíz:
```env
SQL_PASSWORD=Tu_Contraseña_Super_Segura_123!
REDIS_PASSWORD=Tu_Redis_Password_456!
```

Actualizar `docker-compose.yml`:
```yaml
environment:
  - SA_PASSWORD=${SQL_PASSWORD}
```

---

## 📊 Monitoreo

### Health Check de los servicios

```bash
# SQL Server
docker exec ligalibre-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourStrong@Passw0rd -Q "SELECT @@VERSION"

# Redis
docker exec ligalibre-redis redis-cli ping

# LocalStack
curl http://localhost:4566/_localstack/health
```

---

## 🚀 Alternativas

### Usar servicios en la nube

Si prefieres no usar Docker:

- **SQL Server**: Azure SQL Database, AWS RDS
- **Redis**: Azure Cache for Redis, AWS ElastiCache, Redis Cloud
- **SQS**: AWS SQS real (no LocalStack)

---

## 📚 Recursos Adicionales

- [Docker Compose Documentation](https://docs.docker.com/compose/)
- [SQL Server Docker](https://hub.docker.com/_/microsoft-mssql-server)
- [Redis Docker](https://hub.docker.com/_/redis)
- [LocalStack Documentation](https://docs.localstack.cloud/)

---

**Última actualización**: Diciembre 2024
