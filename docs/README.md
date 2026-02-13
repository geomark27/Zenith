# Zenith - Sistema de Gestión de Recursos Humanos

## Descripción

Zenith es un sistema integral de gestión de recursos humanos (HRMS) desarrollado con ASP.NET Core, diseñado para centralizar y automatizar la administración del capital humano en organizaciones.

## Propósito

Proporcionar una plataforma unificada que permita a las empresas gestionar de manera eficiente:

- **Gestión de Personal**: Registro completo de empleados con datos personales, departamentales y contractuales
- **Control de Asistencia**: Marcación de entrada/salida, seguimiento de horas trabajadas y ausentismo
- **Nómina**: Cálculo automatizado de salarios, bonos, deducciones y horas extras
- **Estructura Organizacional**: Administración de departamentos y jerarquías
- **Catálogos Centralizados**: Sistema unificado de estados y clasificaciones para mantener consistencia

## Arquitectura Multi-Tenant

Zenith soporta múltiples organizaciones (tenants) en una única instancia, permitiendo:

- Aislamiento completo de datos entre empresas
- Gestión independiente de catálogos por organización
- Escalabilidad para proveedores de servicios SaaS

## Stack Tecnológico

- **Backend**: ASP.NET Core (.NET 10)
- **Base de Datos**: SQL Server
- **ORM**: Entity Framework Core
- **Arquitectura**: Clean Architecture con separación en capas (API, Application, Infrastructure, Core)

## Estructura del Proyecto

```
Zenith/
├── Zenith.API          # Endpoints REST
├── Zenith.Application  # Lógica de negocio y servicios
├── Zenith.Infrastructure # Acceso a datos y DbContext
└── Zenith.Core         # Entidades y DTOs
```

## Módulos Principales

### Employees (Empleados)
Gestión completa del ciclo de vida del empleado desde contratación hasta desvinculación.

### Departments (Departamentos)
Organización jerárquica con asignación de managers y seguimiento de equipos.

### Attendance (Asistencia)
Control diario de marcaciones, cálculo de horas trabajadas y gestión de ausencias.

### Payroll (Nómina)
Procesamiento de pagos periódicos con cálculo automático de conceptos salariales.

### Catalogs (Catálogos)
Sistema centralizado de estados y clasificaciones para evitar inconsistencias (ej: estados de pago, métodos de pago, estados de asistencia).

## Características Técnicas

- **Auditoría**: Registro automático de creación y modificación con tracking de usuarios
- **Multi-tenant**: Soporte nativo para múltiples organizaciones
- **API RESTful**: Endpoints estandarizados con respuestas estructuradas
- **DTOs**: Separación clara entre modelos de dominio y transferencia de datos
- **Validaciones**: Sistema de validación en múltiples capas

## Roadmap Futuro

- Autenticación JWT y gestión de roles
- Portal de autogestión para empleados
- Reportes y dashboards ejecutivos
- Integración con sistemas de nómina bancaria
- Gestión de evaluaciones de desempeño
- Módulo de reclutamiento integrado con DVRA

---

**Desarrollado por**: Azentic Systems  
**Proyecto**: Expansión del ecosistema de gestión de talento (DVRA → Zenith)
