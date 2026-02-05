using Zenith.Core.DTOs.Payroll;

namespace Zenith.Application.Interfaces
{
    public interface IPayrollService
    {
        Task<IEnumerable<PayrollResponseDto>> GetAllAsync(int tenantId, DateTime? startDate = null, DateTime? endDate = null);
        Task<IEnumerable<PayrollResponseDto>> GetByEmployeeIdAsync(int employeeId, int tenantId);
        Task<PayrollResponseDto?> GetByIdAsync(int id, int tenantId);
        Task<PayrollResponseDto> CreateAsync(CreatePayrollDto dto, int userId);
        Task<PayrollResponseDto?> UpdateAsync(int id, UpdatePayrollDto dto, int tenantId, int userId);
        Task<bool> DeleteAsync(int id, int tenantId);
    }
}