using Zenith.Core.DTOs.Department;

namespace Zenith.Application.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentResponseDto>> GetAllAsync(int tenantId);
        Task<DepartmentDetailResponseDto?> GetByIdAsync(int id, int tenantId, int employeePage = 1, int employeePageSize = 25);
        Task<DepartmentDetailResponseDto> CreateAsync(CreateDepartmentDto dto, int userId);
        Task<DepartmentDetailResponseDto?> UpdateAsync(int id, UpdateDepartmentDto dto, int tenantId, int userId);
        Task<bool> DeleteAsync(int id, int tenantId);
    }
}