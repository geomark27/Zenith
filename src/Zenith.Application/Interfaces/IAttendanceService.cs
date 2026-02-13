using Zenith.Core.DTOs.Attendance;

namespace Zenith.Application.Interfaces
{
    public interface IAttendanceService
    {
        Task<IEnumerable<AttendanceResponseDto>> GetAllAsync(int tenantId, DateTime? startDate = null, DateTime? endDate = null);
        Task<IEnumerable<AttendanceResponseDto>> GetByEmployeeIdAsync(int employeeId, int tenantId);
        Task<AttendanceResponseDto?> GetByIdAsync(int id, int tenantId);
        Task<AttendanceResponseDto> CreateAsync(CreateAttendanceDto dto, int userId);
        Task<AttendanceResponseDto?> UpdateAsync(int id, UpdateAttendanceDto dto, int tenantId, int userId);
        Task<bool> DeleteAsync(int id, int tenantId);
    }
}