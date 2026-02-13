using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Zenith.Application.Interfaces;
using Zenith.Core.DTOs.Employee;
using Zenith.Core.Entities;
using Zenith.Infrastructure.Data;

namespace Zenith.Application.Services
{
    public class EmployeeService(ZenithDbContext context) : IEmployeeService
    {
        private static readonly Expression<Func<Employee, EmployeeResponseDto>> MapToDto = e => new EmployeeResponseDto
        {
            Id          = e.Id,
            FirstName   = e.FirstName,
            LastName    = e.LastName,
            Email       = e.Email,
            Phone       = e.Phone,
            DateOfBirth = e.DateOfBirth,
            HireDate    = e.HireDate,
            Department = new EmployeeDepartmentDto
            {
                Id          = e.Department.Id,
                Name        = e.Department.Name,
                Description = e.Department.Description
            },
            Position    = e.Position,
            Salary      = e.Salary,
            IsActive    = e.IsActive
        };

        public async Task<IEnumerable<EmployeeResponseDto>> GetAllAsync(int tenantId)
        {
            return await context.Employees
                .Where(e => e.TenantId == tenantId)
                .Select(MapToDto)
                .ToListAsync();
        }

        public async Task<EmployeeResponseDto?> GetByIdAsync(int id, int tenantId)
        {
            return await context.Employees
                .Where(e => e.Id == id && e.TenantId == tenantId)
                .Select(MapToDto)
                .FirstOrDefaultAsync();
        }

        public async Task<EmployeeResponseDto?> CreateAsync(CreateEmployeeDto dto, int userId)
        {
            var employee = new Employee
            {
                FirstName       = dto.FirstName,
                LastName        = dto.LastName,
                Email           = dto.Email,
                Phone           = dto.Phone,
                DateOfBirth     = dto.DateOfBirth,
                HireDate        = dto.HireDate,
                DepartmentId    = dto.DepartmentId,
                Position        = dto.Position,
                Salary          = dto.Salary,
                TenantId        = dto.TenantId,
                IsActive        = true,
                CreatedAt       = DateTime.UtcNow,
                CreatedById     = userId
            };

            context.Employees.Add(employee);
            await context.SaveChangesAsync();

            return await GetByIdAsync(employee.Id, dto.TenantId);
        }

        public async Task<EmployeeResponseDto?> UpdateAsync(int id, UpdateEmployeeDto dto, int tenantId, int userId)
        {
            var employee = await context.Employees
                .FirstOrDefaultAsync(e => e.Id == id && e.TenantId == tenantId);

            if (employee == null)
                return null;

            if (dto.FirstName != null)       employee.FirstName      = dto.FirstName;
            if (dto.LastName != null)        employee.LastName       = dto.LastName;
            if (dto.Phone != null)           employee.Phone          = dto.Phone;
            if (dto.DepartmentId.HasValue)   employee.DepartmentId   = dto.DepartmentId.Value;
            if (dto.Position != null)        employee.Position       = dto.Position;
            if (dto.Salary.HasValue)         employee.Salary         = dto.Salary.Value;
            if (dto.IsActive.HasValue)       employee.IsActive       = dto.IsActive.Value;
            
            employee.UpdatedAt      = DateTime.UtcNow;
            employee.UpdatedById    = userId;

            await context.SaveChangesAsync();

            return await GetByIdAsync(id, tenantId);
        }

        public async Task<bool> DeleteAsync(int id, int tenantId)
        {
            var employee = await context.Employees
                .FirstOrDefaultAsync(e => e.Id == id && e.TenantId == tenantId);

            if (employee == null)
                return false;

            context.Employees.Remove(employee);
            await context.SaveChangesAsync();

            return true;
        }
    }
}