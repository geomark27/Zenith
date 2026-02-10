using Microsoft.EntityFrameworkCore;
using Zenith.Application.Interfaces;
using Zenith.Core.DTOs.Department;
using Zenith.Core.Entities;
using Zenith.Infrastructure.Data;

namespace Zenith.Application.Services
{
    public class DepartmentService(ZenithDbContext context) : IDepartmentService
    {
        private readonly ZenithDbContext _context = context;
        
        public async Task<IEnumerable<DepartmentResponseDto>> GetAllAsync(int tenantId)
        {
            return await _context.Departments
                .Where(d => d.TenantId == tenantId)
                .Select(d => new DepartmentResponseDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    ManagerId = d.ManagerId,
                    ManagerName = d.ManagerId != null 
                        ? _context.Employees
                            .Where(e => e.Id == d.ManagerId)
                            .Select(e => e.FirstName + " " + e.LastName)
                            .FirstOrDefault()
                        : null,
                    EmployeeCount = d.Employees.Count,
                    CreatedAt = d.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<DepartmentResponseDto?> GetByIdAsync(int id, int tenantId)
        {
            return await _context.Departments
                .Where(d => d.Id == id && d.TenantId == tenantId)
                .Select(d => new DepartmentResponseDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    ManagerId = d.ManagerId,
                    ManagerName = d.ManagerId != null 
                        ? _context.Employees
                            .Where(e => e.Id == d.ManagerId)
                            .Select(e => e.FirstName + " " + e.LastName)
                            .FirstOrDefault()
                        : null,
                    EmployeeCount = d.Employees.Count,
                    CreatedAt = d.CreatedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<DepartmentResponseDto> CreateAsync(CreateDepartmentDto dto, int userId)
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

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return (await GetByIdAsync(department.Id, dto.TenantId))!;
        }

        public async Task<DepartmentResponseDto?> UpdateAsync(int id, UpdateDepartmentDto dto, int tenantId, int userId)
        {
            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.Id == id && d.TenantId == tenantId);

            if (department == null)
                return null;

            if (dto.Name != null) department.Name = dto.Name;
            if (dto.Description != null) department.Description = dto.Description;
            if (dto.ManagerId.HasValue) department.ManagerId = dto.ManagerId;
            
            department.UpdatedAt = DateTime.UtcNow;
            department.UpdatedById = userId;

            await _context.SaveChangesAsync();

            return await GetByIdAsync(id, tenantId);
        }

        public async Task<bool> DeleteAsync(int id, int tenantId)
        {
            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.Id == id && d.TenantId == tenantId);

            if (department == null)
                return false;

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}