# Zenith - Sistema de Gestión de Recursos Humanos

Sistema multi-tenant para gestión de empleados, asistencias y nóminas desarrollado con .NET 10 y SQL Server.

## 🏗️ Arquitectura

El proyecto sigue una arquitectura en capas (Clean Architecture):

```
Zenith/
├── Zenith.API/            # Capa de presentación (Controllers, Endpoints)
├── Zenith.Application/    # Capa de aplicación (Servicios, DTOs, Use Cases)
├── Zenith.Core/           # Capa de dominio (Entidades, Interfaces)
└── Zenith.Infrastructure/ # Capa de infraestructura (DbContext, Repositorios)
```

## 📋 Requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [SQL Server 2022](https://www.microsoft.com/sql-server) (o SQL Server Express)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) / [VS Code](https://code.visualstudio.com/)

## 🚀 Configuración Local

### 1. Clonar el repositorio

```bash
git clone https://github.com/tu-usuario/zenith.git
cd zenith
```

### 2. Configurar la base de datos

El proyecto usa SQL Server con Windows Authentication por defecto. Verifica la cadena de conexión en `Zenith.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ZenithDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 3. Aplicar migraciones

```bash
dotnet ef database update --project Zenith.Infrastructure --startup-project Zenith.API
```

### 4. Ejecutar la aplicación

```bash
dotnet run --project Zenith.API
```

La API estará disponible en:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`
- Swagger UI: `https://localhost:5001/swagger`

## 🗄️ Modelo de Datos

| Entidad | Descripción |
|---------|-------------|
| `Tenant` | Empresa/Organización (multi-tenancy) |
| `Employee` | Empleados de la organización |
| `Department` | Departamentos de la empresa |
| `Attendance` | Registro de asistencias |
| `Payroll` | Nóminas y pagos |
| `Catalog` | Catálogos configurables (estados, métodos de pago, etc.) |

## 🛠️ Comandos Útiles

### Entity Framework

```bash
# Crear nueva migración
dotnet ef migrations add NombreMigracion --project Zenith.Infrastructure --startup-project Zenith.API

# Aplicar migraciones
dotnet ef database update --project Zenith.Infrastructure --startup-project Zenith.API

# Revertir última migración
dotnet ef migrations remove --project Zenith.Infrastructure --startup-project Zenith.API

# Eliminar base de datos
dotnet ef database drop --project Zenith.Infrastructure --startup-project Zenith.API
```

### Build y Run

```bash
# Restaurar paquetes
dotnet restore

# Compilar
dotnet build

# Ejecutar en modo desarrollo
dotnet run --project Zenith.API

# Ejecutar tests
dotnet test
```

## 🔧 Configuración para Docker (Opcional)

Si prefieres usar Docker, crea un archivo `.env` en la raíz:

```env
DB_SERVER=localhost,1433
DB_NAME=ZenithDB
DB_USER=sa
DB_PASSWORD=YourStrong@Password123
```

## 📝 Licencia

Este proyecto está bajo la licencia MIT.
