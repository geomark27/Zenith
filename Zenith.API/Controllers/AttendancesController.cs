using Microsoft.AspNetCore.Mvc;
using Zenith.Application.Interfaces;
using Zenith.Core.DTOs.Attendance;

namespace Zenith.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendancesController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendancesController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AttendanceResponseDto>>> GetAll(
            [FromQuery] int tenantId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var results = await _attendanceService.GetAllAsync(tenantId, startDate, endDate);
            return Ok(results);
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<IEnumerable<AttendanceResponseDto>>> GetByEmployeeId(int employeeId, [FromQuery] int tenantId)
        {
            var results = await _attendanceService.GetByEmployeeIdAsync(employeeId, tenantId);
            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AttendanceResponseDto>> GetById(int id, [FromQuery] int tenantId)
        {
            var attendance = await _attendanceService.GetByIdAsync(id, tenantId);
            if (attendance == null)
                return NotFound();

            return Ok(attendance);
        }

        [HttpPost]
        public async Task<ActionResult<AttendanceResponseDto>> Create([FromBody] CreateAttendanceDto dto)
        {
            int userId = 1;
            var attendance = await _attendanceService.CreateAsync(dto, userId);
            if (attendance == null)
                return BadRequest();

            return CreatedAtAction(nameof(GetById), new { id = attendance.Id, tenantId = dto.TenantId }, attendance);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AttendanceResponseDto>> Update(int id, [FromBody] UpdateAttendanceDto dto, [FromQuery] int tenantId)
        {
            int userId = 1;
            var attendance = await _attendanceService.UpdateAsync(id, dto, tenantId, userId);
            if (attendance == null)
                return NotFound();

            return Ok(attendance);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] int tenantId)
        {
            var result = await _attendanceService.DeleteAsync(id, tenantId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
