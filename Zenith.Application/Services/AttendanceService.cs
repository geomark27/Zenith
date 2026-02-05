using Microsoft.EntityFrameworkCore;
using Zenith.Application.Interfaces;
using Zenith.Core.DTOs.Attendance;
using Zenith.Core.Entities;
using Zenith.Infrastructure.Data;

namespace Zenith.Application.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly ZenithDbContext _context;

        public AttendanceService(ZenithDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AttendanceResponseDto>> GetAllAsync(int tenantId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Attendances
                .Where(a => a.TenantId == tenantId);

            if (startDate.HasValue)
                query = query.Where(a => a.Date >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(a => a.Date <= endDate.Value);

            return await query
                .Include(a => a.Employee)
                .Include(a => a.StatusCatalog)
                .Select(a => new AttendanceResponseDto
                {
                    Id = a.Id,
                    EmployeeId = a.EmployeeId,
                    EmployeeName = a.Employee.FirstName + " " + a.Employee.LastName,
                    Date = a.Date,
                    CheckInTime = a.CheckInTime,
                    CheckOutTime = a.CheckOutTime,
                    WorkedHours = a.WorkedHours,
                    StatusCatalogId = a.StatusCatalogId,
                    StatusName = a.StatusCatalog.Value,
                    Notes = a.Notes,
                    CreatedAt = a.CreatedAt
                })
                .OrderByDescending(a => a.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<AttendanceResponseDto>> GetByEmployeeIdAsync(int employeeId, int tenantId)
        {
            return await _context.Attendances
                .Where(a => a.EmployeeId == employeeId && a.TenantId == tenantId)
                .Include(a => a.Employee)
                .Include(a => a.StatusCatalog)
                .Select(a => new AttendanceResponseDto
                {
                    Id = a.Id,
                    EmployeeId = a.EmployeeId,
                    EmployeeName = a.Employee.FirstName + " " + a.Employee.LastName,
                    Date = a.Date,
                    CheckInTime = a.CheckInTime,
                    CheckOutTime = a.CheckOutTime,
                    WorkedHours = a.WorkedHours,
                    StatusCatalogId = a.StatusCatalogId,
                    StatusName = a.StatusCatalog.Value,
                    Notes = a.Notes,
                    CreatedAt = a.CreatedAt
                })
                .OrderByDescending(a => a.Date)
                .ToListAsync();
        }

        public async Task<AttendanceResponseDto?> GetByIdAsync(int id, int tenantId)
        {
            return await _context.Attendances
                .Where(a => a.Id == id && a.TenantId == tenantId)
                .Include(a => a.Employee)
                .Include(a => a.StatusCatalog)
                .Select(a => new AttendanceResponseDto
                {
                    Id = a.Id,
                    EmployeeId = a.EmployeeId,
                    EmployeeName = a.Employee.FirstName + " " + a.Employee.LastName,
                    Date = a.Date,
                    CheckInTime = a.CheckInTime,
                    CheckOutTime = a.CheckOutTime,
                    WorkedHours = a.WorkedHours,
                    StatusCatalogId = a.StatusCatalogId,
                    StatusName = a.StatusCatalog.Value,
                    Notes = a.Notes,
                    CreatedAt = a.CreatedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<AttendanceResponseDto> CreateAsync(CreateAttendanceDto dto, int userId)
        {
            var attendance = new Attendance
            {
                EmployeeId = dto.EmployeeId,
                Date = dto.Date,
                CheckInTime = dto.CheckInTime,
                CheckOutTime = dto.CheckOutTime,
                WorkedHours = dto.WorkedHours,
                StatusCatalogId = dto.StatusCatalogId,
                Notes = dto.Notes,
                TenantId = dto.TenantId,
                CreatedAt = DateTime.UtcNow,
                CreatedById = userId
            };

            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();

            return (await GetByIdAsync(attendance.Id, dto.TenantId))!;
        }

        public async Task<AttendanceResponseDto?> UpdateAsync(int id, UpdateAttendanceDto dto, int tenantId, int userId)
        {
            var attendance = await _context.Attendances
                .FirstOrDefaultAsync(a => a.Id == id && a.TenantId == tenantId);

            if (attendance == null)
                return null;

            if (dto.CheckInTime.HasValue) attendance.CheckInTime = dto.CheckInTime;
            if (dto.CheckOutTime.HasValue) attendance.CheckOutTime = dto.CheckOutTime;
            if (dto.WorkedHours.HasValue) attendance.WorkedHours = dto.WorkedHours;
            if (dto.StatusCatalogId.HasValue) attendance.StatusCatalogId = dto.StatusCatalogId.Value;
            if (dto.Notes != null) attendance.Notes = dto.Notes;
            
            attendance.UpdatedAt = DateTime.UtcNow;
            attendance.UpdatedById = userId;

            await _context.SaveChangesAsync();

            return await GetByIdAsync(id, tenantId);
        }

        public async Task<bool> DeleteAsync(int id, int tenantId)
        {
            var attendance = await _context.Attendances
                .FirstOrDefaultAsync(a => a.Id == id && a.TenantId == tenantId);

            if (attendance == null)
                return false;

            _context.Attendances.Remove(attendance);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}