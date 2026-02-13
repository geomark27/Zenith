using Zenith.Core.DTOs.Employee;

namespace Zenith.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeResponseDto>> GetAllAsync(int tenantId);
        Task<EmployeeResponseDto?> GetByIdAsync(int id, int tenantId);
        Task<EmployeeResponseDto?> CreateAsync(CreateEmployeeDto dto, int userId);
        Task<EmployeeResponseDto?> UpdateAsync(int id, UpdateEmployeeDto dto, int tenantId, int userId);
        Task<bool> DeleteAsync(int id, int tenantId);
    }
}