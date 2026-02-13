using Microsoft.EntityFrameworkCore;
using Zenith.Application.Interfaces;
using Zenith.Core.DTOs.Department;
using Zenith.Core.Entities;
using Zenith.Infrastructure.Data;

namespace Zenith.Application.Services
{
    public class DepartmentService(ZenithDbContext context) : IDepartmentService
    {
        public async Task<IEnumerable<DepartmentResponseDto>> GetAllAsync(int tenantId)
        {
            return await context.Departments
                .Where(d => d.TenantId == tenantId)
                .Select(d => new DepartmentResponseDto
                {
                    Id          = d.Id,
                    Name        = d.Name,
                    Description = d.Description,
                    ManagerId   = d.ManagerId,
                    ManagerName = d.ManagerId != null 
                        ? context.Employees
                            .Where(e => e.Id == d.ManagerId)
                            .Select(e => e.FirstName + " " + e.LastName)
                            .FirstOrDefault()
                        : null,
                    EmployeeCount   = d.Employees.Count,
                    CreatedAt       = d.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<DepartmentDetailResponseDto?> GetByIdAsync(int id, int tenantId, int employeePage = 1, int employeePageSize = 25)
        {
            var department = await context.Departments
                .Where(d => d.Id == id && d.TenantId == tenantId)
                .Select(d => new DepartmentDetailResponseDto
                {
                    Id          = d.Id,
                    Name        = d.Name,
                    Description = d.Description,
                    ManagerId   = d.ManagerId,
                    ManagerName = d.ManagerId != null
                        ? context.Employees
                            .Where(e => e.Id == d.ManagerId)
                            .Select(e => e.FirstName + " " + e.LastName)
                            .FirstOrDefault()
                        : null,
                    EmployeeCount   = d.Employees.Count,
                    CreatedAt       = d.CreatedAt,
                    Employees       = d.Employees
                        .OrderBy(e => e.FirstName)
                        .Skip((employeePage - 1) * employeePageSize)
                        .Take(employeePageSize)
                        .Select(e => new DepartmentEmployeeDto
                        {
                            Id        = e.Id,
                            FirstName = e.FirstName,
                            LastName  = e.LastName,
                            Email     = e.Email,
                            Position  = e.Position,
                            IsActive  = e.IsActive
                        })
                        .ToList(),
                    EmployeePage        = employeePage,
                    EmployeePageSize    = employeePageSize
                })
                .FirstOrDefaultAsync();

            if (department != null)
                department.EmployeeTotalPages = (int)Math.Ceiling(department.EmployeeCount / (double)employeePageSize);

            return department;
        }

        public async Task<DepartmentDetailResponseDto> CreateAsync(CreateDepartmentDto dto, int userId)
        {
            var department = new Department
            {
                Name = dto.Name,
                Description = dto.Description,
                ManagerId = dto.ManagerId,
                TenantId = dto.TenantId,
                CreatedAt = DateTime.UtcNow,
                CreatedById = userId
            };

            context.Departments.Add(department);
            await context.SaveChangesAsync();

            return (await GetByIdAsync(department.Id, dto.TenantId))!;
        }

        public async Task<DepartmentDetailResponseDto?> UpdateAsync(int id, UpdateDepartmentDto dto, int tenantId, int userId)
        {
            var department = await context.Departments
                .FirstOrDefaultAsync(d => d.Id == id && d.TenantId == tenantId);

            if (department == null)
                return null;

            if (dto.Name != null) department.Name = dto.Name;
            if (dto.Description != null) department.Description = dto.Description;
            if (dto.ManagerId.HasValue) department.ManagerId = dto.ManagerId;
            
            department.UpdatedAt = DateTime.UtcNow;
            department.UpdatedById = userId;

            await context.SaveChangesAsync();

            return await GetByIdAsync(id, tenantId);
        }

        public async Task<bool> DeleteAsync(int id, int tenantId)
        {
            var department = await context.Departments
                .FirstOrDefaultAsync(d => d.Id == id && d.TenantId == tenantId);

            if (department == null)
                return false;

            context.Departments.Remove(department);
            await context.SaveChangesAsync();

            return true;
        }
    }
}