using Microsoft.EntityFrameworkCore;
using Zenith.Application.Interfaces;
using Zenith.Core.DTOs.Employee;
using Zenith.Core.Entities;
using Zenith.Infrastructure.Data;

namespace Zenith.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ZenithDbContext _context;

        public EmployeeService(ZenithDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmployeeResponseDto>> GetAllAsync(int tenantId)
        {
            return await _context.Employees
                .Where(e => e.TenantId == tenantId)
                .Include(e => e.Department)
                .Select(e => new EmployeeResponseDto
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    Phone = e.Phone,
                    DateOfBirth = e.DateOfBirth,
                    HireDate = e.HireDate,
                    DepartmentId = e.DepartmentId,
                    DepartmentName = e.Department.Name,
                    Position = e.Position,
                    Salary = e.Salary,
                    IsActive = e.IsActive
                })
                .ToListAsync();
        }

        public async Task<EmployeeResponseDto?> GetByIdAsync(int id, int tenantId)
        {
            return await _context.Employees
                .Where(e => e.Id == id && e.TenantId == tenantId)
                .Include(e => e.Department)
                .Select(e => new EmployeeResponseDto
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    Phone = e.Phone,
                    DateOfBirth = e.DateOfBirth,
                    HireDate = e.HireDate,
                    DepartmentId = e.DepartmentId,
                    DepartmentName = e.Department.Name,
                    Position = e.Position,
                    Salary = e.Salary,
                    IsActive = e.IsActive
                })
                .FirstOrDefaultAsync();
        }

        public async Task<EmployeeResponseDto?> CreateAsync(CreateEmployeeDto dto, int userId)
        {
            var employee = new Employee
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                DateOfBirth = dto.DateOfBirth,
                HireDate = dto.HireDate,
                DepartmentId = dto.DepartmentId,
                Position = dto.Position,
                Salary = dto.Salary,
                TenantId = dto.TenantId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedById = userId
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(employee.Id, dto.TenantId);
        }

        public async Task<EmployeeResponseDto?> UpdateAsync(int id, UpdateEmployeeDto dto, int tenantId, int userId)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == id && e.TenantId == tenantId);

            if (employee == null)
                return null;

            employee.FirstName = dto.FirstName;
            employee.LastName = dto.LastName;
            employee.Phone = dto.Phone;
            employee.DepartmentId = dto.DepartmentId;
            employee.Position = dto.Position;
            employee.Salary = dto.Salary;
            employee.IsActive = dto.IsActive;
            employee.UpdatedAt = DateTime.UtcNow;
            employee.UpdatedById = userId;

            await _context.SaveChangesAsync();

            return await GetByIdAsync(id, tenantId);
        }

        public async Task<bool> DeleteAsync(int id, int tenantId)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == id && e.TenantId == tenantId);

            if (employee == null)
                return false;

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}