# Zenith — Sprint Planning & Progress Tracker

> Última actualización: 2026-02-22

---

## Progreso General

| Sprint | Descripción | Avance |
|---|---|---|
| Sprint 1 | Infraestructura y Setup | ✅ 100% |
| Sprint 2 | Dominio Core (Entidades y DTOs) | ✅ 100% |
| Sprint 3 | Módulo Employees | ✅ 100% |
| Sprint 4 | Módulo Departments | ✅ 100% |
| Sprint 5 | Módulo Attendances | ✅ 100% |
| Sprint 6 | Módulo Payrolls | ✅ 100% |
| Sprint 7 | Módulo Catalogs | ✅ 100% |
| Sprint 8 | Autenticación y Autorización | ❌ 0% |
| Sprint 9 | Testing | ❌ 0% |
| Sprint 10 | Features Avanzados / UX | ❌ 0% |

**Progreso total**: ~70% (7/10 sprints completados)

---

## Sprint 1 — Infraestructura y Setup

**Estado**: ✅ Completo

| Tarea | Estado |
|---|---|
| Crear solución .NET con 4 proyectos (API, Application, Core, Infrastructure) | ✅ |
| Configurar referencias entre proyectos | ✅ |
| Configurar EF Core + SQL Server | ✅ |
| Configurar docker-compose para SQL Server 2022 | ✅ |
| Configurar DotNetEnv para variables de entorno | ✅ |
| Configurar Swagger / Swashbuckle | ✅ |
| Configurar rutas en minúsculas (`LowercaseUrls`) | ✅ |
| Seed data: Tenant y User admin por defecto | ✅ |
| Migration inicial (`InitialCreate`) | ✅ |

---

## Sprint 2 — Dominio Core (Entidades y DTOs)

**Estado**: ✅ Completo

| Tarea | Estado |
|---|---|
| Entidad Tenant | ✅ |
| Entidad User | ✅ |
| Entidad Employee | ✅ |
| Entidad Department | ✅ |
| Entidad Attendance | ✅ |
| Entidad Payroll | ✅ |
| Entidad Catalog | ✅ |
| ApiResponse<T> envelope estándar | ✅ |
| PagedResponse (definido, uso parcial) | ✅ |
| Configuración DbContext con índices y restricciones FK | ✅ |
| Precisión decimal en campos monetarios y horas | ✅ |

---

## Sprint 3 — Módulo Employees

**Estado**: ✅ Completo

| Tarea | Estado |
|---|---|
| IEmployeeService (interfaz) | ✅ |
| EmployeeService: GetAllAsync filtrado por tenant | ✅ |
| EmployeeService: GetByIdAsync | ✅ |
| EmployeeService: CreateAsync | ✅ |
| EmployeeService: UpdateAsync (partial update) | ✅ |
| EmployeeService: DeleteAsync | ✅ |
| CreateEmployeeDto con validaciones | ✅ |
| UpdateEmployeeDto (campos opcionales) | ✅ |
| EmployeeResponseDto con Department anidado | ✅ |
| EmployeesController: GET /api/employees | ✅ |
| EmployeesController: GET /api/employees/{id} | ✅ |
| EmployeesController: POST /api/employees | ✅ |
| EmployeesController: PUT /api/employees/{id} | ✅ |
| EmployeesController: DELETE /api/employees/{id} | ✅ |

---

## Sprint 4 — Módulo Departments

**Estado**: ✅ Completo

| Tarea | Estado |
|---|---|
| IDepartmentService (interfaz) | ✅ |
| DepartmentService: GetAllAsync con conteo de empleados | ✅ |
| DepartmentService: GetByIdAsync con lista paginada de empleados | ✅ |
| DepartmentService: CreateAsync | ✅ |
| DepartmentService: UpdateAsync | ✅ |
| DepartmentService: DeleteAsync | ✅ |
| CreateDepartmentDto | ✅ |
| UpdateDepartmentDto | ✅ |
| DepartmentResponseDto (listado) | ✅ |
| DepartmentDetailResponseDto (paginación de empleados) | ✅ |
| DepartmentEmployeeDto | ✅ |
| DepartmentsController: CRUD completo | ✅ |
| Paginación de empleados en GetById (page + pageSize) | ✅ |

---

## Sprint 5 — Módulo Attendances

**Estado**: ✅ Completo

| Tarea | Estado |
|---|---|
| IAttendanceService (interfaz) | ✅ |
| AttendanceService: GetAllAsync con filtro de fechas | ✅ |
| AttendanceService: GetByEmployeeIdAsync | ✅ |
| AttendanceService: GetByIdAsync | ✅ |
| AttendanceService: CreateAsync | ✅ |
| AttendanceService: UpdateAsync (partial update) | ✅ |
| AttendanceService: DeleteAsync | ✅ |
| CreateAttendanceDto | ✅ |
| UpdateAttendanceDto | ✅ |
| AttendanceResponseDto | ✅ |
| AttendancesController: GET /api/attendances | ✅ |
| AttendancesController: GET /api/attendances/employee/{id} | ✅ |
| AttendancesController: GET /api/attendances/{id} | ✅ |
| AttendancesController: POST | ✅ |
| AttendancesController: PUT /{id} | ✅ |
| AttendancesController: DELETE /{id} | ✅ |

---

## Sprint 6 — Módulo Payrolls

**Estado**: ✅ Completo

| Tarea | Estado |
|---|---|
| IPayrollService (interfaz) | ✅ |
| PayrollService: GetAllAsync con filtro de fechas | ✅ |
| PayrollService: GetByEmployeeIdAsync | ✅ |
| PayrollService: GetByIdAsync | ✅ |
| PayrollService: CreateAsync | ✅ |
| PayrollService: UpdateAsync | ✅ |
| PayrollService: DeleteAsync | ✅ |
| CreatePayrollDto | ✅ |
| UpdatePayrollDto | ✅ |
| PayrollResponseDto | ✅ |
| PayrollsController: CRUD completo | ✅ |
| PayrollsController: GET /api/payrolls/employee/{id} | ✅ |

---

## Sprint 7 — Módulo Catalogs

**Estado**: ✅ Completo

| Tarea | Estado |
|---|---|
| ICatalogService (interfaz) | ✅ |
| CatalogService: GetAllAsync | ✅ |
| CatalogService: GetByCategoryAsync (solo activos) | ✅ |
| CatalogService: GetByIdAsync | ✅ |
| CatalogService: GetByCodeAsync | ✅ |
| CatalogService: CreateAsync | ✅ |
| CatalogService: UpdateAsync | ✅ |
| CatalogService: DeleteAsync | ✅ |
| CreateCatalogDto | ✅ |
| UpdateCatalogDto | ✅ |
| CatalogResponseDto | ✅ |
| CatalogsController: GET /api/catalogs | ✅ |
| CatalogsController: GET /api/catalogs/category/{category} | ✅ |
| CatalogsController: GET /api/catalogs/code/{code} | ✅ |
| CatalogsController: GET /api/catalogs/{id} | ✅ |
| CatalogsController: POST / PUT / DELETE | ✅ |

---

## Sprint 8 — Autenticación y Autorización

**Estado**: ❌ Pendiente (0%)

| Tarea | Estado |
|---|---|
| Implementar JWT authentication en Program.cs | ❌ |
| Crear AuthController (login, refresh token) | ❌ |
| Crear AuthService + IAuthService | ❌ |
| Hashear contraseñas (BCrypt o similar) — seed usa texto plano | ❌ |
| Agregar `[Authorize]` a todos los controllers | ❌ |
| Extraer `userId` del token JWT (actualmente hardcodeado en 1) | ❌ |
| Middleware de autorización por rol (Admin, Manager, Employee) | ❌ |
| Crear DTOs: LoginRequestDto, LoginResponseDto (token + user info) | ❌ |

---

## Sprint 9 — Testing

**Estado**: ❌ Pendiente (0%)

| Tarea | Estado |
|---|---|
| Unit tests para EmployeeService | ❌ |
| Unit tests para DepartmentService | ❌ |
| Unit tests para AttendanceService | ❌ |
| Unit tests para PayrollService | ❌ |
| Unit tests para CatalogService | ❌ |
| Integration tests para endpoints | ❌ |
| Configurar test project en solución | ❌ |

---

## Sprint 10 — Features Avanzados

**Estado**: ❌ Pendiente (0%)

| Tarea | Estado |
|---|---|
| Cálculo automático de NetPay en PayrollService | ❌ |
| Cálculo automático de WorkedHours en AttendanceService | ❌ |
| Endpoint de nómina masiva (bulk create por período) | ❌ |
| Endpoint de reportes de asistencia por período | ❌ |
| Endpoint de reportes de costos laborales por departamento | ❌ |
| Portal de autogestión para empleados | ❌ |
| Integración con cliente mobile/frontend | ❌ |
| TenantController (CRUD de organizaciones) | ❌ |
| UserController (CRUD de usuarios del sistema) | ❌ |

---

## Tabla de Endpoints vs Integración Cliente

| Endpoint | Método | Implementado (API) | Integrado Cliente |
|---|---|:---:|:---:|
| /api/employees | GET | ✅ | <!-- TODO: verificar --> ❓ |
| /api/employees/{id} | GET | ✅ | ❓ |
| /api/employees | POST | ✅ | ❓ |
| /api/employees/{id} | PUT | ✅ | ❓ |
| /api/employees/{id} | DELETE | ✅ | ❓ |
| /api/departments | GET | ✅ | ❓ |
| /api/departments/{id} | GET | ✅ | ❓ |
| /api/departments | POST | ✅ | ❓ |
| /api/departments/{id} | PUT | ✅ | ❓ |
| /api/departments/{id} | DELETE | ✅ | ❓ |
| /api/attendances | GET | ✅ | ❓ |
| /api/attendances/employee/{id} | GET | ✅ | ❓ |
| /api/attendances/{id} | GET | ✅ | ❓ |
| /api/attendances | POST | ✅ | ❓ |
| /api/attendances/{id} | PUT | ✅ | ❓ |
| /api/attendances/{id} | DELETE | ✅ | ❓ |
| /api/payrolls | GET | ✅ | ❓ |
| /api/payrolls/employee/{id} | GET | ✅ | ❓ |
| /api/payrolls/{id} | GET | ✅ | ❓ |
| /api/payrolls | POST | ✅ | ❓ |
| /api/payrolls/{id} | PUT | ✅ | ❓ |
| /api/payrolls/{id} | DELETE | ✅ | ❓ |
| /api/catalogs | GET | ✅ | ❓ |
| /api/catalogs/category/{cat} | GET | ✅ | ❓ |
| /api/catalogs/code/{code} | GET | ✅ | ❓ |
| /api/catalogs/{id} | GET | ✅ | ❓ |
| /api/catalogs | POST | ✅ | ❓ |
| /api/catalogs/{id} | PUT | ✅ | ❓ |
| /api/catalogs/{id} | DELETE | ✅ | ❓ |
| /api/auth/login | POST | ❌ | ❌ |
| /api/tenants | GET/POST/... | ❌ | ❌ |
| /api/users | GET/POST/... | ❌ | ❌ |

> ❓ = No hay cliente identificado en el repo actual. Actualizar cuando se integre el frontend/mobile.
