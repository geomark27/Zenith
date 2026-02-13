using Microsoft.AspNetCore.Mvc;
using Zenith.Application.Interfaces;
using Zenith.Core.Common;
using Zenith.Core.DTOs.Attendance;

namespace Zenith.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendancesController(IAttendanceService attendanceService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<AttendanceResponseDto>>>> GetAll(
            [FromQuery] int tenantId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var attendances = await attendanceService.GetAllAsync(tenantId, startDate, endDate);
            return Ok(ApiResponse<IEnumerable<AttendanceResponseDto>>.SuccessResponse(
                attendances, 
                attendances.Count(), 
                "Attendances retrieved successfully"
            ));
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<AttendanceResponseDto>>>> GetByEmployeeId(int employeeId, [FromQuery] int tenantId)
        {
            var attendances = await attendanceService.GetByEmployeeIdAsync(employeeId, tenantId);
            return Ok(ApiResponse<IEnumerable<AttendanceResponseDto>>.SuccessResponse(
                attendances, 
                attendances.Count(), 
                "Attendances retrieved successfully"
            ));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<AttendanceResponseDto>>> GetById(int id, [FromQuery] int tenantId)
        {
            var attendance = await attendanceService.GetByIdAsync(id, tenantId);
            
            if (attendance == null)
                return NotFound(ApiResponse<AttendanceResponseDto>.ErrorResponse("Attendance not found"));

            return Ok(ApiResponse<AttendanceResponseDto>.SuccessResponse(attendance, "Attendance retrieved successfully"));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<AttendanceResponseDto>>> Create([FromBody] CreateAttendanceDto dto)
        {
            int userId = 1;
            var attendance = await attendanceService.CreateAsync(dto, userId);
            
            if (attendance == null)
                return BadRequest(ApiResponse<AttendanceResponseDto>.ErrorResponse("Failed to create attendance"));

            return CreatedAtAction(
                nameof(GetById), 
                new { id = attendance.Id, tenantId = dto.TenantId }, 
                ApiResponse<AttendanceResponseDto>.SuccessResponse(attendance, "Attendance created successfully")
            );
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<AttendanceResponseDto>>> Update(int id, [FromBody] UpdateAttendanceDto dto, [FromQuery] int tenantId)
        {
            int userId = 1;
            var attendance = await attendanceService.UpdateAsync(id, dto, tenantId, userId);
            
            if (attendance == null)
                return NotFound(ApiResponse<AttendanceResponseDto>.ErrorResponse("Attendance not found"));

            return Ok(ApiResponse<AttendanceResponseDto>.SuccessResponse(attendance, "Attendance updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id, [FromQuery] int tenantId)
        {
            var result = await attendanceService.DeleteAsync(id, tenantId);
            if (!result)
                return NotFound(ApiResponse<object>.ErrorResponse("Attendance not found"));

            return Ok(ApiResponse<object>.SuccessResponse(null!, "Attendance deleted successfully"));
        }
    }
}
