using Zenith.Core.DTOs.Department;

namespace Zenith.Application.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentResponseDto>> GetAllAsync(int tenantId);
        Task<DepartmentResponseDto?> GetByIdAsync(int id, int tenantId);
        Task<DepartmentResponseDto> CreateAsync(CreateDepartmentDto dto, int userId);
        Task<DepartmentResponseDto?> UpdateAsync(int id, UpdateDepartmentDto dto, int tenantId, int userId);
        Task<bool> DeleteAsync(int id, int tenantId);
    }
}