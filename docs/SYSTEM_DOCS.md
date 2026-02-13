# Zenith - Sistema de Gestión de Recursos Humanos

## 📋 Tabla de Contenidos

- [Descripción General](#descripción-general)
- [Arquitectura Multi-Tenant](#arquitectura-multi-tenant)
- [Módulos del Sistema](#módulos-del-sistema)
  - [Tenant (Organización)](#tenant-organización)
  - [User (Usuario)](#user-usuario)
  - [Employee (Empleado)](#employee-empleado)
  - [Department (Departamento)](#department-departamento)
  - [Attendance (Asistencia)](#attendance-asistencia)
  - [Payroll (Nómina)](#payroll-nómina)
  - [Catalog (Catálogo)](#catalog-catálogo)
- [Flujos de Trabajo](#flujos-de-trabajo)
- [Auditoría y Trazabilidad](#auditoría-y-trazabilidad)

---

## Descripción General

**Zenith** es un sistema integral de gestión de recursos humanos (HRMS) diseñado para centralizar y automatizar todos los procesos relacionados con la administración del capital humano en organizaciones de cualquier tamaño.

### Propósito

Proporcionar una plataforma unificada que permita:

- **Gestión completa del ciclo de vida del empleado**: Desde contratación hasta desvinculación
- **Control preciso de asistencia**: Marcación diaria, cálculo de horas trabajadas y gestión de ausencias
- **Procesamiento automatizado de nómina**: Cálculo de salarios, bonos, deducciones y conceptos variables
- **Organización departamental**: Estructura jerárquica con gestión de equipos y managers
- **Estandarización de datos**: Sistema de catálogos centralizado para mantener consistencia
- **Soporte multi-organización**: Arquitectura multi-tenant para proveedores SaaS o grupos empresariales

### Stack Tecnológico

- **Backend**: ASP.NET Core (.NET 10)
- **Base de Datos**: SQL Server
- **ORM**: Entity Framework Core
- **Arquitectura**: Clean Architecture (API → Application → Infrastructure → Core)

---

## Arquitectura Multi-Tenant

Zenith implementa multi-tenancy a nivel de aplicación, permitiendo que múltiples organizaciones (tenants) compartan la misma instancia del sistema mientras mantienen sus datos completamente aislados.

**Características:**
- Cada tenant representa una organización independiente
- Aislamiento total de datos mediante `TenantId`
- Catálogos personalizables por organización
- Subdominios únicos para identificación
- Escalabilidad para modelos SaaS

---

## Módulos del Sistema

### Tenant (Organización)

**Responsabilidad**: Representa una organización o empresa que utiliza el sistema.

**Funcionalidad Principal**:
- Registro de información corporativa (nombre, identificación fiscal, contacto)
- Asignación de subdominios únicos para acceso
- Control de estado activo/inactivo
- Punto de aislamiento para arquitectura multi-tenant

**Campos Clave**:
- `Name`: Nombre comercial de la organización
- `Subdomain`: Identificador único para acceso (ej: `empresa.zenith.app`)
- `TaxId`: RUC o identificación fiscal
- `Email`, `Phone`, `Address`: Datos de contacto corporativo
- `IsActive`: Control de habilitación del tenant

**Relaciones**:
- Contiene múltiples: Employees, Departments, Catalogs, Users

---

### User (Usuario)

**Responsabilidad**: Gestión de usuarios del sistema con autenticación y roles.

**Funcionalidad Principal**:
- Autenticación mediante email y contraseña hasheada
- Control de acceso basado en roles (RBAC)
- Gestión de permisos por tipo de usuario
- Tracking de quién crea/modifica registros

**Campos Clave**:
- `Email`: Identificador único de autenticación
- `PasswordHash`: Contraseña encriptada
- `FirstName`, `LastName`: Información personal
- `Role`: Tipo de usuario (Admin, Manager, Employee)
- `IsActive`: Estado de habilitación del usuario
- `TenantId`: Organización a la que pertenece

**Roles del Sistema**:
- **Admin**: Acceso total al sistema, gestión de configuraciones
- **Manager**: Gestión de equipos, aprobaciones, reportes
- **Employee**: Acceso limitado a datos propios (autogestión)

**Relaciones**:
- Pertenece a: Tenant
- Crea/Modifica: Todos los registros del sistema (auditoría)

---

### Employee (Empleado)

**Responsabilidad**: Registro central de información de empleados.

**Funcionalidad Principal**:
- Almacenamiento de datos personales y contractuales
- Vinculación con departamentos
- Control de estado activo/inactivo
- Base para asistencia y nómina

**Campos Clave**:
- `FirstName`, `LastName`: Identificación personal
- `Email`, `Phone`: Contacto
- `DateOfBirth`: Fecha de nacimiento
- `HireDate`: Fecha de contratación
- `Position`: Cargo o puesto
- `Salary`: Salario base
- `IsActive`: Estado laboral (activo/inactivo)
- `DepartmentId`: Departamento asignado
- `TenantId`: Organización empleadora

**Relaciones**:
- Pertenece a: Department, Tenant
- Tiene múltiples: Attendances, Payrolls
- Creado/Modificado por: User

**Casos de Uso**:
- Alta de nuevo empleado
- Actualización de información contractual
- Reasignación de departamento
- Cambio de salario
- Desvinculación (IsActive = false)

---

### Department (Departamento)

**Responsabilidad**: Organización jerárquica de la estructura empresarial.

**Funcionalidad Principal**:
- Agrupación de empleados por área funcional
- Asignación de responsables (managers)
- Facilitar reportes y análisis por departamento

**Campos Clave**:
- `Name`: Nombre del departamento (ej: "Sistemas", "Ventas", "RRHH")
- `Description`: Descripción de funciones o responsabilidades
- `ManagerId`: Empleado responsable del departamento (opcional)
- `TenantId`: Organización propietaria

**Relaciones**:
- Pertenece a: Tenant
- Contiene múltiples: Employees
- Manager es: Employee (relación opcional)
- Creado/Modificado por: User

**Casos de Uso**:
- Crear estructura organizacional
- Asignar manager a departamento
- Reasignar empleados entre departamentos
- Reportes de headcount por área
- Análisis de costos laborales por departamento

---

### Attendance (Asistencia)

**Responsabilidad**: Control diario de marcación y horas trabajadas.

**Funcionalidad Principal**:
- Registro de entrada/salida de empleados
- Cálculo automático de horas trabajadas
- Gestión de estados (presente, ausente, tardanza, permiso)
- Base de datos para cálculo de horas extras en nómina

**Campos Clave**:
- `EmployeeId`: Empleado asociado
- `Date`: Fecha del registro
- `CheckInTime`: Hora de entrada
- `CheckOutTime`: Hora de salida
- `WorkedHours`: Horas calculadas (CheckOut - CheckIn)
- `StatusCatalogId`: Estado de asistencia (Catalog)
- `Notes`: Observaciones o justificaciones
- `TenantId`: Organización

**Estados Típicos (vía Catalog)**:
- `PRESENT`: Asistencia normal
- `ABSENT`: Ausencia no justificada
- `LATE`: Llegada tardía
- `ON_LEAVE`: Permiso autorizado
- `SICK_LEAVE`: Incapacidad médica

**Relaciones**:
- Pertenece a: Employee, Tenant
- Usa: StatusCatalog para clasificación
- Creado/Modificado por: User

**Casos de Uso**:
- Marcación de entrada/salida diaria
- Registro de ausencias
- Cálculo de horas extras
- Reportes de ausentismo
- Validación de tardanzas
- Justificación de inasistencias

**Flujo Típico**:
1. Empleado marca entrada → `CheckInTime` registrado
2. Empleado marca salida → `CheckOutTime` registrado
3. Sistema calcula → `WorkedHours = CheckOutTime - CheckInTime`
4. Sistema determina → `StatusCatalogId` según reglas (tardanza, ausencia, etc.)

---

### Payroll (Nómina)

**Responsabilidad**: Procesamiento y registro de pagos a empleados.

**Funcionalidad Principal**:
- Cálculo de salarios por período
- Gestión de conceptos variables (bonos, horas extras, deducciones)
- Control de estados de pago
- Registro de métodos de pago
- Historial de pagos por empleado

**Campos Clave**:
- `EmployeeId`: Empleado al que se paga
- `PayPeriodStart`, `PayPeriodEnd`: Rango del período (quincenal, mensual)
- `PaymentDate`: Fecha programada de pago
- `BaseSalary`: Salario base del empleado
- `Bonuses`: Bonificaciones del período
- `OvertimePay`: Pago por horas extras (calculado desde Attendance)
- `Deductions`: Descuentos (impuestos, préstamos, seguros)
- `NetPay`: Salario neto (BaseSalary + Bonuses + OvertimePay - Deductions)
- `StatusCatalogId`: Estado del pago (Catalog)
- `PaymentMethodCatalogId`: Método de pago (Catalog)
- `TenantId`: Organización

**Estados de Pago (vía Catalog)**:
- `PENDING`: Nómina creada, pendiente de procesar
- `APPROVED`: Nómina aprobada, lista para pago
- `PAID`: Pago ejecutado
- `REJECTED`: Nómina rechazada
- `CANCELLED`: Nómina cancelada

**Métodos de Pago (vía Catalog)**:
- `BANK_TRANSFER`: Transferencia bancaria
- `CASH`: Efectivo
- `CHECK`: Cheque

**Relaciones**:
- Pertenece a: Employee, Tenant
- Usa: StatusCatalog, PaymentMethodCatalog
- Creado/Modificado por: User
- Calcula desde: Attendance (horas extras)

**Casos de Uso**:
- Generar nómina quincenal/mensual masiva
- Calcular horas extras desde asistencia
- Aplicar bonos por desempeño
- Registrar deducciones (préstamos, ISR, seguro)
- Procesar pagos
- Generar recibos de nómina
- Reportes de costos laborales

**Flujo de Procesamiento**:
1. **Creación**: Admin/Manager crea nómina para período específico
   - `Status = PENDING`
   - Sistema toma `BaseSalary` del Employee
2. **Cálculo**: Sistema calcula conceptos variables
   - Consulta Attendance para horas extras → `OvertimePay`
   - Aplica bonos → `Bonuses`
   - Aplica deducciones → `Deductions`
   - Calcula → `NetPay`
3. **Aprobación**: Manager revisa y aprueba → `Status = APPROVED`
4. **Pago**: Se ejecuta transferencia → `Status = PAID`
5. **Historial**: Registro permanente para auditoría

---

### Catalog (Catálogo)

**Responsabilidad**: Centralización de valores de dominio y clasificaciones.

**Funcionalidad Principal**:
- Estandarizar estados y clasificaciones en todo el sistema
- Evitar valores hardcodeados o strings sueltos
- Mantener consistencia entre módulos
- Soportar jerarquías de clasificación
- Personalización por tenant

**Campos Clave**:
- `Name`: Nombre descriptivo del catálogo
- `Code`: Código único identificador (ej: `PAY_STATUS_PAID`)
- `Category`: Agrupación temática (ej: "Payment", "Attendance")
- `Value`: Texto mostrado al usuario (ej: "Paid", "Presente")
- `Description`: Explicación del concepto
- `ParentId`: Catálogo padre para jerarquías (opcional)
- `Order`: Orden de despliegue en UI
- `IsActive`: Control de vigencia
- `TenantId`: Permite catálogos personalizados por organización

**Categorías Principales**:

| Category | Ejemplos de Codes |
|----------|-------------------|
| Payment | PAY_STATUS_PAID, PAY_STATUS_PENDING, PAY_METHOD_BANK |
| Attendance | ATT_STATUS_PRESENT, ATT_STATUS_ABSENT, ATT_STATUS_LATE |
| Employee | EMP_STATUS_ACTIVE, EMP_STATUS_INACTIVE |
| Department | DEPT_TYPE_OPERATIONAL, DEPT_TYPE_ADMINISTRATIVE |

**Relaciones**:
- Pertenece a: Tenant
- Usado por: Attendance (StatusCatalog), Payroll (StatusCatalog, PaymentMethodCatalog)
- Jerarquía: Parent → Children (auto-referencial)
- Creado/Modificado por: User

**Ventajas del Sistema de Catálogos**:
1. **Prevención de errores**: No más typos ("Paid" vs "paid" vs "Pagado")
2. **Flexibilidad**: Agregar nuevos estados sin cambiar código
3. **Multi-idioma**: Value puede cambiar por idioma manteniendo Code constante
4. **Auditoría**: Tracking de cambios en clasificaciones
5. **Validaciones**: Garantizar solo valores permitidos

**Casos de Uso**:
- Crear catálogo de estados de pago para Payroll
- Definir tipos de ausencias para Attendance
- Configurar métodos de pago aceptados
- Establecer jerarquías (ej: tipos de bonos)
- Activar/desactivar valores sin eliminar historial

**Ejemplo de Jerarquía**:
```
BONUS (ParentId: null)
├── PERFORMANCE_BONUS (ParentId: BONUS.Id)
├── ATTENDANCE_BONUS (ParentId: BONUS.Id)
└── SENIORITY_BONUS (ParentId: BONUS.Id)
```

---

## Flujos de Trabajo

### 1. Alta de Empleado

1. Admin crea registro en **Employee**
2. Asigna a **Department**
3. Sistema crea usuario en **User** (opcional, según rol)
4. Empleado queda disponible para **Attendance** y **Payroll**

### 2. Procesamiento de Nómina Mensual

1. Manager crea registros de **Payroll** para todos los empleados activos
2. Sistema consulta **Attendance** del período para calcular horas extras
3. Sistema calcula conceptos:
   - `BaseSalary` desde **Employee**
   - `OvertimePay` desde **Attendance**
   - Aplica `Bonuses` y `Deductions`
   - Calcula `NetPay`
4. Manager aprueba nómina → Status = APPROVED
5. Finance ejecuta pago → Status = PAID

### 3. Control de Asistencia Diario

1. Empleado marca entrada → **Attendance.CheckInTime**
2. Empleado marca salida → **Attendance.CheckOutTime**
3. Sistema calcula → `WorkedHours`
4. Sistema determina → `StatusCatalogId` (presente, tardanza, etc.)
5. Manager revisa excepciones (ausencias, tardanzas)
6. Datos quedan disponibles para cálculo de nómina

### 4. Gestión de Catálogos

1. Admin define catálogos necesarios por **Category**
2. Crea valores con **Code** únicos
3. Módulos referencian catálogos por `CatalogId`
4. Admin puede activar/desactivar valores sin afectar historial

---

## Auditoría y Trazabilidad

Todos los módulos principales implementan auditoría completa:

**Campos de Auditoría**:
- `CreatedAt`: Fecha de creación del registro
- `UpdatedAt`: Última fecha de modificación
- `CreatedById`: Usuario que creó el registro
- `UpdatedById`: Usuario que realizó la última modificación

**Beneficios**:
- Trazabilidad completa de cambios
- Cumplimiento normativo
- Detección de anomalías
- Análisis forense de datos
- Responsabilidad por acciones

---

## Resumen de Capacidades

✅ **Gestión de Personal**: Registro completo de empleados con estructura departamental
✅ **Control de Asistencia**: Marcación diaria con cálculo automático de horas
✅ **Procesamiento de Nómina**: Cálculo de salarios con conceptos variables
✅ **Multi-Tenant**: Soporte para múltiples organizaciones aisladas
✅ **Catálogos Centralizados**: Estandarización de clasificaciones
✅ **Auditoría Completa**: Tracking de cambios y responsables
✅ **Arquitectura Escalable**: Clean Architecture con separación de capas

---

**Desarrollado por**: Azentic Systems
**Versión**: 1.0
**Última Actualización**: Febrero 2026
