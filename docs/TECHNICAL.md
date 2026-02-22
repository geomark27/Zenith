# Zenith — Documentación Técnica

> Última actualización: 2026-02-22

---

## Stack y Dependencias

| Componente | Tecnología |
|---|---|
| Runtime | .NET 10 |
| Framework Web | ASP.NET Core (Minimal API + Controllers) |
| ORM | Entity Framework Core 10 |
| Base de Datos | SQL Server 2022 (Express en Docker) |
| Contenedores | Docker + Docker Compose |
| Documentación API | Swagger / Swashbuckle 10.1.1 |
| Autenticación | `Microsoft.AspNetCore.Authentication.JwtBearer` (instalado, no configurado aún) |
| Variables de entorno | DotNetEnv 3.1.1 |

### Paquetes por proyecto

**Zenith.API**
- `Swashbuckle.AspNetCore` 10.1.1
- `Microsoft.AspNetCore.OpenApi` 10.0.2
- `Microsoft.AspNetCore.Authentication.JwtBearer` 10.0.2
- `DotNetEnv` 3.1.1
- `Microsoft.EntityFrameworkCore.Design` 10.0.2
- `Microsoft.Extensions.Configuration.EnvironmentVariables` 10.0.2

**Zenith.Infrastructure**
- Entity Framework Core + SQL Server Provider

---

## Estructura de Módulos / Capas

```
Zenith/
├── src/
│   ├── Zenith.API/              # Capa de presentación: Controllers, Program.cs
│   │   └── Controllers/
│   │       ├── AttendancesController.cs
│   │       ├── CatalogsController.cs
│   │       ├── DepartmentsController.cs
│   │       ├── EmployeesController.cs
│   │       └── PayrollsController.cs
│   │
│   ├── Zenith.Application/      # Lógica de negocio: Services e Interfaces
│   │   ├── Interfaces/
│   │   │   ├── IAttendanceService.cs
│   │   │   ├── ICatalogService.cs
│   │   │   ├── IDepartmentService.cs
│   │   │   ├── IEmployeeService.cs
│   │   │   └── IPayrollService.cs
│   │   └── Services/
│   │       ├── AttendanceService.cs
│   │       ├── CatalogService.cs
│   │       ├── DepartmentService.cs
│   │       ├── EmployeeService.cs
│   │       └── PayrollService.cs
│   │
│   ├── Zenith.Core/             # Dominio: Entities, DTOs, Common
│   │   ├── Common/
│   │   │   ├── ApiResponse.cs
│   │   │   └── PagedResponse.cs
│   │   ├── DTOs/
│   │   │   ├── Attendance/
│   │   │   ├── Catalog/
│   │   │   ├── Department/
│   │   │   ├── Employee/
│   │   │   └── Payroll/
│   │   └── Entities/
│   │       ├── Attendance.cs
│   │       ├── Catalog.cs
│   │       ├── Department.cs
│   │       ├── Employee.cs
│   │       ├── Payroll.cs
│   │       ├── Tenant.cs
│   │       └── User.cs
│   │
│   └── Zenith.Infrastructure/   # Infraestructura: DbContext, Migrations
│       ├── Data/
│       │   └── ZenithDbContext.cs
│       └── Migrations/
│
├── docs/                        # Documentación del proyecto
├── docker-compose.yml           # SQL Server en Docker
└── Zenith.slnx                  # Solution file
```

### Descripción de capas

**Zenith.API**: Punto de entrada HTTP. Los controladores reciben requests, delegan al servicio correspondiente y retornan `ApiResponse<T>` estandarizado. Las rutas se generan en minúsculas (`LowercaseUrls = true`).

**Zenith.Application**: Contiene toda la lógica de negocio. Los servicios implementan interfaces definidas en este mismo proyecto. Acceden directamente al `ZenithDbContext` de Infrastructure (sin Repository Pattern intermedio).

**Zenith.Core**: Proyecto de dominio puro. No tiene dependencias externas. Define entidades, DTOs de entrada/salida y el envelope de respuesta `ApiResponse<T>`.

**Zenith.Infrastructure**: Configuración de EF Core, DbContext y Migrations. Incluye seed data para el tenant y usuario admin por defecto.

---

## Patrón de Respuesta Estándar

Todos los endpoints retornan `ApiResponse<T>`:

```json
{
  "success": true,
  "message": "string",
  "data": <T>,
  "count": 0  // presente solo en listados
}
```

---

## Configuración de Base de Datos

La conexión se resuelve en `Program.cs` con prioridad:
1. Variables de entorno (`DB_SERVER`, `DB_NAME`, `DB_USER`, `DB_PASSWORD`) — para Docker/producción.
2. `appsettings.json` → `ConnectionStrings:DefaultConnection` — para desarrollo local.

El docker-compose levanta SQL Server 2022 Express en el puerto configurado por `DB_PORT`.

**Seed Data** (aplicado en migrations):
- Tenant ID=1: AZENTIC SYS (azenticsys.com)
- User ID=1: admin@azenticsys.com / ADMIN

---

## Patrones de Arquitectura

- **Clean Architecture**: separación en 4 capas con dependencias unidireccionales (API → Application → Infrastructure, todos dependen de Core).
- **Dependency Injection**: servicios registrados como `Scoped` en `Program.cs`.
- **DTO Pattern**: entidades de dominio nunca expuestas directamente; se usan DTOs específicos para Request y Response.
- **Expression-based Projection**: uso de `Expression<Func<T, TDto>>` estático en EmployeeService para mapeo eficiente sin materializar entidades completas.
- **Multi-tenancy a nivel de aplicación**: filtrado por `TenantId` en cada consulta (sin Row-Level Security en DB).

---

## Endpoints

### Módulo: Employees — `api/employees`

#### GET /api/employees
Lista todos los empleados de un tenant.

**Query params**:
- `tenantId` (int, requerido)

**Response** `200`:
```json
{
  "success": true,
  "message": "Employees retrieved successfully",
  "data": [
    {
      "id": 1,
      "firstName": "string",
      "lastName": "string",
      "email": "string",
      "phone": "string",
      "dateOfBirth": "2026-01-01T00:00:00Z",
      "hireDate": "2026-01-01T00:00:00Z",
      "position": "string",
      "salary": 0.00,
      "isActive": true,
      "department": {
        "id": 1,
        "name": "string",
        "description": "string"
      }
    }
  ],
  "count": 1
}
```

```bash
curl -X GET "http://localhost:5000/api/employees?tenantId=1"
```

---

#### GET /api/employees/{id}
Obtiene un empleado por ID.

**Path params**: `id` (int)
**Query params**: `tenantId` (int, requerido)

**Response** `200`: objeto EmployeeResponseDto
**Response** `404`: `{ "success": false, "message": "Employee not found" }`

```bash
curl -X GET "http://localhost:5000/api/employees/1?tenantId=1"
```

---

#### POST /api/employees
Crea un nuevo empleado.

**Body**:
```json
{
  "firstName": "string",     // requerido
  "lastName": "string",      // requerido
  "email": "string",         // requerido
  "phone": "string",         // requerido
  "dateOfBirth": "2000-01-01T00:00:00Z",
  "hireDate": "2026-01-01T00:00:00Z",
  "departmentId": 1,
  "position": "string",      // requerido
  "salary": 1500.00,
  "tenantId": 1              // requerido
}
```

**Response** `201`: EmployeeResponseDto
**Response** `400`: Error al crear

```bash
curl -X POST "http://localhost:5000/api/employees" \
  -H "Content-Type: application/json" \
  -d '{"firstName":"Juan","lastName":"Perez","email":"juan@empresa.com","phone":"0999999999","dateOfBirth":"1990-01-01T00:00:00Z","hireDate":"2026-02-01T00:00:00Z","departmentId":1,"position":"Developer","salary":2000,"tenantId":1}'
```

---

#### PUT /api/employees/{id}
Actualiza parcialmente un empleado (todos los campos son opcionales).

**Path params**: `id` (int)
**Query params**: `tenantId` (int)

**Body** (todos opcionales):
```json
{
  "firstName": "string",
  "lastName": "string",
  "phone": "string",
  "departmentId": 1,
  "position": "string",
  "salary": 0.00,
  "isActive": true
}
```

**Response** `200`: EmployeeResponseDto actualizado
**Response** `404`: No encontrado

```bash
curl -X PUT "http://localhost:5000/api/employees/1?tenantId=1" \
  -H "Content-Type: application/json" \
  -d '{"salary":2500,"isActive":true}'
```

---

#### DELETE /api/employees/{id}
Elimina físicamente un empleado.

**Path params**: `id` (int)
**Query params**: `tenantId` (int)

**Response** `200`: `{ "success": true, "message": "Employee deleted successfully" }`
**Response** `404`: No encontrado

```bash
curl -X DELETE "http://localhost:5000/api/employees/1?tenantId=1"
```

---

### Módulo: Departments — `api/departments`

#### GET /api/departments
Lista todos los departamentos de un tenant.

**Query params**: `tenantId` (int, requerido)

**Response** incluye: `id`, `name`, `description`, `managerId`, `managerName`, `employeeCount`, `createdAt`

```bash
curl -X GET "http://localhost:5000/api/departments?tenantId=1"
```

---

#### GET /api/departments/{id}
Obtiene detalle de un departamento con lista paginada de empleados.

**Query params**:
- `tenantId` (int, requerido)
- `employeePage` (int, default: 1)
- `employeePageSize` (int, default: 25)

**Response** incluye además: `employees[]`, `employeePage`, `employeePageSize`, `employeeTotalPages`

```bash
curl -X GET "http://localhost:5000/api/departments/1?tenantId=1&employeePage=1&employeePageSize=10"
```

---

#### POST /api/departments

**Body**:
```json
{
  "name": "string",        // requerido
  "description": "string", // opcional
  "managerId": 1,          // opcional
  "tenantId": 1            // requerido
}
```

```bash
curl -X POST "http://localhost:5000/api/departments" \
  -H "Content-Type: application/json" \
  -d '{"name":"Sistemas","description":"Área de tecnología","tenantId":1}'
```

---

#### PUT /api/departments/{id}

**Body** (todos opcionales): `name`, `description`, `managerId`

---

#### DELETE /api/departments/{id}

**Query params**: `tenantId` (int)

---

### Módulo: Attendances — `api/attendances`

#### GET /api/attendances
Lista asistencias de un tenant con filtro opcional por fecha.

**Query params**:
- `tenantId` (int, requerido)
- `startDate` (DateTime, opcional)
- `endDate` (DateTime, opcional)

**Response**: ordenado por fecha descendente.

```bash
curl -X GET "http://localhost:5000/api/attendances?tenantId=1&startDate=2026-02-01&endDate=2026-02-28"
```

---

#### GET /api/attendances/employee/{employeeId}
Lista asistencias de un empleado específico.

**Path params**: `employeeId` (int)
**Query params**: `tenantId` (int)

```bash
curl -X GET "http://localhost:5000/api/attendances/employee/1?tenantId=1"
```

---

#### GET /api/attendances/{id}

**Query params**: `tenantId` (int)

**Response**:
```json
{
  "id": 1,
  "employeeId": 1,
  "employeeName": "Juan Perez",
  "date": "2026-02-22T00:00:00Z",
  "checkInTime": "2026-02-22T08:00:00Z",
  "checkOutTime": "2026-02-22T17:00:00Z",
  "workedHours": 9.00,
  "statusCatalogId": 1,
  "statusName": "Presente",
  "notes": null,
  "createdAt": "2026-02-22T08:00:00Z"
}
```

---

#### POST /api/attendances

**Body**:
```json
{
  "employeeId": 1,              // requerido
  "date": "2026-02-22T00:00:00Z", // requerido
  "checkInTime": "2026-02-22T08:00:00Z",
  "checkOutTime": "2026-02-22T17:00:00Z",
  "workedHours": 9.00,
  "statusCatalogId": 1,         // requerido
  "notes": "string",
  "tenantId": 1                 // requerido
}
```

```bash
curl -X POST "http://localhost:5000/api/attendances" \
  -H "Content-Type: application/json" \
  -d '{"employeeId":1,"date":"2026-02-22T00:00:00Z","checkInTime":"2026-02-22T08:00:00Z","checkOutTime":"2026-02-22T17:00:00Z","workedHours":9,"statusCatalogId":1,"tenantId":1}'
```

---

#### PUT /api/attendances/{id}

**Body** (todos opcionales): `checkInTime`, `checkOutTime`, `workedHours`, `statusCatalogId`, `notes`

---

#### DELETE /api/attendances/{id}

**Query params**: `tenantId` (int)

---

### Módulo: Payrolls — `api/payrolls`

#### GET /api/payrolls
Lista nóminas con filtro opcional por período.

**Query params**:
- `tenantId` (int, requerido)
- `startDate` (DateTime, opcional) — filtra por `PayPeriodStart >= startDate`
- `endDate` (DateTime, opcional) — filtra por `PayPeriodEnd <= endDate`

```bash
curl -X GET "http://localhost:5000/api/payrolls?tenantId=1&startDate=2026-02-01&endDate=2026-02-28"
```

---

#### GET /api/payrolls/employee/{employeeId}

**Query params**: `tenantId` (int)

---

#### GET /api/payrolls/{id}

**Response**:
```json
{
  "id": 1,
  "employeeId": 1,
  "employeeName": "Juan Perez",
  "payPeriodStart": "2026-02-01T00:00:00Z",
  "payPeriodEnd": "2026-02-28T00:00:00Z",
  "paymentDate": "2026-02-28T00:00:00Z",
  "baseSalary": 2000.00,
  "bonuses": 200.00,
  "overtimePay": 150.00,
  "deductions": 100.00,
  "netPay": 2250.00,
  "statusCatalogId": 1,
  "statusName": "Pending",
  "paymentMethodCatalogId": 1,
  "paymentMethodName": "Bank Transfer",
  "createdAt": "2026-02-22T00:00:00Z"
}
```

---

#### POST /api/payrolls

**Body**:
```json
{
  "employeeId": 1,                         // requerido
  "payPeriodStart": "2026-02-01T00:00:00Z", // requerido
  "payPeriodEnd": "2026-02-28T00:00:00Z",   // requerido
  "paymentDate": "2026-02-28T00:00:00Z",    // requerido
  "baseSalary": 2000.00,                    // requerido
  "bonuses": 200.00,
  "overtimePay": 150.00,
  "deductions": 100.00,
  "netPay": 2250.00,                        // requerido
  "statusCatalogId": 1,                     // requerido
  "paymentMethodCatalogId": 1,              // requerido
  "tenantId": 1                             // requerido
}
```

---

#### PUT /api/payrolls/{id}

**Body** (todos opcionales): `paymentDate`, `bonuses`, `overtimePay`, `deductions`, `netPay`, `statusCatalogId`, `paymentMethodCatalogId`

---

#### DELETE /api/payrolls/{id}

---

### Módulo: Catalogs — `api/catalogs`

#### GET /api/catalogs
Lista todos los catálogos de un tenant, ordenados por Category y Order.

**Query params**: `tenantId` (int, requerido)

```bash
curl -X GET "http://localhost:5000/api/catalogs?tenantId=1"
```

---

#### GET /api/catalogs/category/{category}
Lista catálogos activos de una categoría específica.

**Path params**: `category` (string, ej: "Payment", "Attendance")
**Query params**: `tenantId` (int)

```bash
curl -X GET "http://localhost:5000/api/catalogs/category/Attendance?tenantId=1"
```

---

#### GET /api/catalogs/code/{code}
Obtiene un catálogo por su código único.

**Path params**: `code` (string, ej: "PAY_STATUS_PAID")
**Query params**: `tenantId` (int)

```bash
curl -X GET "http://localhost:5000/api/catalogs/code/PAY_STATUS_PAID?tenantId=1"
```

---

#### GET /api/catalogs/{id}

**Response**:
```json
{
  "id": 1,
  "code": "PAY_STATUS_PAID",
  "category": "Payment",
  "value": "Paid",
  "description": "Pago ejecutado",
  "parentId": null,
  "parentValue": null,
  "order": 1,
  "isActive": true
}
```

---

#### POST /api/catalogs

**Body**:
```json
{
  "name": "string",        // requerido
  "code": "string",        // requerido, único por tenant
  "category": "string",    // requerido
  "value": "string",       // requerido
  "description": "string", // requerido
  "parentId": null,        // opcional, FK a otro Catalog
  "order": 0,
  "tenantId": 1            // requerido
}
```

---

#### PUT /api/catalogs/{id}

**Body** (todos opcionales): `value`, `description`, `order`, `isActive`

> Nota: `code` y `category` no son actualizables una vez creados (no expuestos en UpdateCatalogDto).

---

#### DELETE /api/catalogs/{id}

---

## Modelos / DTOs Relevantes

### Employee
| Campo | Tipo | Requerido |
|---|---|---|
| Id | int | — |
| FirstName | string | ✓ |
| LastName | string | ✓ |
| Email | string | ✓ |
| Phone | string | ✓ |
| DateOfBirth | DateTime | — |
| HireDate | DateTime | — |
| Position | string | ✓ |
| Salary | decimal (10,2) | ✓ |
| IsActive | bool | — |
| DepartmentId | int | ✓ |
| TenantId | int | ✓ |

### Attendance
| Campo | Tipo | Requerido |
|---|---|---|
| EmployeeId | int | ✓ |
| Date | DateTime | ✓ |
| CheckInTime | DateTime? | — |
| CheckOutTime | DateTime? | — |
| WorkedHours | decimal? (5,2) | — |
| StatusCatalogId | int | ✓ |
| Notes | string? | — |
| TenantId | int | ✓ |

### Payroll
| Campo | Tipo | Requerido |
|---|---|---|
| EmployeeId | int | ✓ |
| PayPeriodStart | DateTime | ✓ |
| PayPeriodEnd | DateTime | ✓ |
| PaymentDate | DateTime | ✓ |
| BaseSalary | decimal (10,2) | ✓ |
| Bonuses | decimal? (10,2) | — |
| OvertimePay | decimal? (10,2) | — |
| Deductions | decimal? (10,2) | — |
| NetPay | decimal (10,2) | ✓ |
| StatusCatalogId | int | ✓ |
| PaymentMethodCatalogId | int | ✓ |
| TenantId | int | ✓ |

---

## Códigos de Error

| HTTP | Situación |
|---|---|
| 200 | Operación exitosa |
| 201 | Recurso creado |
| 400 | Datos inválidos o falla de creación |
| 404 | Recurso no encontrado |
| 500 | Error interno del servidor (no mapeado explícitamente) |
