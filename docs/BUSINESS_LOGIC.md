# Zenith — Lógica de Negocio

> Última actualización: 2026-02-22

---

## Propósito del Sistema

Zenith es un sistema de gestión de Recursos Humanos (HRMS) multi-tenant desarrollado por Azentic Systems. Permite a las organizaciones centralizar y automatizar la administración de su capital humano: desde el alta de empleados hasta el procesamiento de nómina, pasando por el control de asistencia y la estructura departamental.

El sistema está diseñado como plataforma SaaS, permitiendo que múltiples empresas operen de forma completamente aislada bajo una misma instancia.

---

## Entidades Principales del Dominio

### Tenant (Organización)
Representa a cada empresa cliente del sistema. Es el punto de aislamiento de todos los datos. Cada registro en el sistema pertenece a un Tenant identificado por `TenantId`. Un Tenant tiene nombre comercial, subdominio de acceso único, identificación fiscal (RUC/NIT), datos de contacto corporativo y estado activo/inactivo.

### User (Usuario del Sistema)
Persona que opera Zenith. Tiene credenciales de acceso (email + contraseña hasheada) y un rol que define sus permisos:
- **Admin**: acceso total, puede gestionar toda la configuración.
- **Manager**: gestión de equipos, aprobación de nóminas y asistencia.
- **Employee**: acceso limitado a sus propios datos (autogestión). <!-- TODO: verificar si el portal de autogestión está implementado -->

Los usuarios son también la fuente de trazabilidad: cada operación de escritura registra quién la realizó (`CreatedById`, `UpdatedById`).

### Employee (Empleado)
Entidad central del dominio. Representa a una persona contratada por la organización. Contiene datos personales (nombre, email, teléfono, fecha de nacimiento), contractuales (cargo, salario base, fecha de contratación) y de estado (activo/inactivo). Está vinculado a un Departamento y es la base sobre la que se construyen Asistencias y Nóminas.

### Department (Departamento)
Unidad organizacional que agrupa empleados por área funcional (Sistemas, Ventas, RRHH, etc.). Puede tener un manager asignado (que es a su vez un Empleado). Facilita la generación de reportes por área y el análisis de costos laborales.

### Attendance (Asistencia)
Registro diario de presencia de un empleado. Captura hora de entrada, hora de salida, horas trabajadas calculadas y el estado de la jornada (presente, ausente, tardanza, permiso, etc.). Es la fuente de datos para el cálculo de horas extras en nómina.

### Payroll (Nómina)
Registro de pago a un empleado por un período determinado. Consolida salario base, bonificaciones, pago por horas extras y deducciones para calcular el salario neto. Tiene un ciclo de vida propio (Pendiente → Aprobado → Pagado) y un método de pago asociado.

### Catalog (Catálogo)
Sistema centralizado de valores de dominio. Evita valores hardcodeados en la lógica de negocio. Cada catálogo tiene un código único (`Code`), una categoría temática (`Category`) y un valor presentable al usuario (`Value`). Soporta jerarquías (catálogos padre/hijo) y es personalizable por Tenant.

Categorías principales:
- **Attendance**: estados de asistencia (PRESENT, ABSENT, LATE, ON_LEAVE, SICK_LEAVE)
- **Payment**: estados de nómina (PENDING, APPROVED, PAID, REJECTED, CANCELLED) y métodos de pago (BANK_TRANSFER, CASH, CHECK)

---

## Flujos de Negocio Clave

### 1. Alta de Empleado

```
Admin crea Employee con datos personales y contractuales
    → Asigna DepartmentId (el departamento debe existir previamente)
    → El empleado queda activo (IsActive = true por defecto)
    → Queda disponible para registrar asistencias y procesar nómina
```

Un empleado no puede existir sin un Departamento válido del mismo Tenant. El email no está restringido a unicidad en este módulo (la unicidad se aplica al User del sistema, no al Employee).

---

### 2. Control de Asistencia Diario

```
Se crea un registro de Attendance para un empleado en una fecha
    → Se registra CheckInTime (entrada)
    → Se registra CheckOutTime (salida)
    → WorkedHours = CheckOutTime - CheckInTime  (calculado manualmente al crear/actualizar)
    → StatusCatalogId indica el tipo de jornada (presente, tardanza, ausencia, etc.)
    → Notes permite registrar justificaciones o notas del supervisor
```

La granularidad es diaria: un registro por empleado por día. Los registros de asistencia son la fuente primaria para calcular horas extras en nómina.

---

### 3. Procesamiento de Nómina

```
Admin/Manager crea un Payroll para un empleado y período
    → Status inicial: PENDING
    → BaseSalary se toma del salary actual del Employee
    → OvertimePay se calcula consultando Attendances del período
    → Se agregan Bonuses y Deductions manualmente
    → NetPay = BaseSalary + Bonuses + OvertimePay - Deductions

Manager revisa y aprueba → Status: APPROVED
Finance ejecuta el pago → Status: PAID
```

El cálculo de `NetPay` es responsabilidad del cliente que crea la nómina; actualmente el servicio persiste el valor que recibe sin recalcularlo automáticamente.

---

### 4. Gestión Organizacional (Departamentos)

```
Admin crea Departments para estructurar la organización
    → Asigna opcionalmente un ManagerId (Employee existente)
    → Los empleados se asignan al departamento en su perfil
    → La vista detallada del departamento incluye lista paginada de empleados
```

Un departamento puede existir sin manager. La paginación de empleados en el detalle de departamento tiene un tamaño por defecto de 25 empleados por página.

---

### 5. Gestión de Catálogos

```
Admin define los valores permitidos para cada categoría
    → Código único por Tenant (Code + TenantId es unique)
    → Los módulos referencian catálogos por ID (FK)
    → Se pueden desactivar valores sin eliminar el historial (IsActive = false)
    → Jerarquía opcional con ParentId para clasificaciones anidadas
```

---

## Reglas de Negocio Detectadas

- **Aislamiento multi-tenant**: Toda consulta filtra por `TenantId`. No existe acceso cross-tenant a nivel de servicio.
- **Soft delete no implementado**: Las eliminaciones son físicas (`Remove` directo sobre el DbContext). No hay papelera ni recuperación.
- **Auditoría obligatoria**: Todos los registros guardan `CreatedAt`, `UpdatedAt`, `CreatedById` y `UpdatedById`.
- **UserId hardcodeado**: Mientras no esté implementada la autenticación JWT, el `userId` se fija en `1` en todos los controladores (usuario admin de seed). <!-- TODO: reemplazar cuando se implemente auth -->
- **Código de catálogo único por tenant**: La combinación `(Code, TenantId)` tiene índice único en base de datos.
- **Índice de asistencia**: `(EmployeeId, Date)` indexado para optimizar consultas por empleado y fecha.
- **Unicidad de usuario**: `(Email, TenantId)` es único para la entidad User.
- **Eliminación restringida**: Las relaciones con Tenant, Catalog, Employee y Department usan `DeleteBehavior.Restrict`, evitando eliminaciones en cascada que rompan el historial.
- **Paginación en departamentos**: `GetById` de Department admite parámetros `employeePage` (default: 1) y `employeePageSize` (default: 25).
- **Filtrado por fecha en Attendance y Payroll**: Los listados admiten `startDate` y `endDate` opcionales.

---

## Roadmap de Negocio (No Implementado)

- Autenticación JWT y gestión de sesiones
- Portal de autogestión para empleados
- Reportes y dashboards ejecutivos
- Integración con sistemas de nómina bancaria
- Evaluaciones de desempeño
- Módulo de reclutamiento (integración con DVRA)
