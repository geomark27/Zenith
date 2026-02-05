using Microsoft.AspNetCore.Mvc;
using Zenith.Application.Interfaces;
using Zenith.Core.DTOs.Payroll;

namespace Zenith.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PayrollsController : ControllerBase
    {
        private readonly IPayrollService _payrollService;

        public PayrollsController(IPayrollService payrollService)
        {
            _payrollService = payrollService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PayrollResponseDto>>> GetAll(
            [FromQuery] int tenantId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var results = await _payrollService.GetAllAsync(tenantId, startDate, endDate);
            return Ok(results);
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<IEnumerable<PayrollResponseDto>>> GetByEmployeeId(int employeeId, [FromQuery] int tenantId)
        {
            var results = await _payrollService.GetByEmployeeIdAsync(employeeId, tenantId);
            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PayrollResponseDto>> GetById(int id, [FromQuery] int tenantId)
        {
            var payroll = await _payrollService.GetByIdAsync(id, tenantId);
            if (payroll == null)
                return NotFound();

            return Ok(payroll);
        }

        [HttpPost]
        public async Task<ActionResult<PayrollResponseDto>> Create([FromBody] CreatePayrollDto dto)
        {
            int userId = 1;
            var payroll = await _payrollService.CreateAsync(dto, userId);
            if (payroll == null)
                return BadRequest();

            return CreatedAtAction(nameof(GetById), new { id = payroll.Id, tenantId = dto.TenantId }, payroll);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PayrollResponseDto>> Update(int id, [FromBody] UpdatePayrollDto dto, [FromQuery] int tenantId)
        {
            int userId = 1;
            var payroll = await _payrollService.UpdateAsync(id, dto, tenantId, userId);
            if (payroll == null)
                return NotFound();

            return Ok(payroll);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] int tenantId)
        {
            var result = await _payrollService.DeleteAsync(id, tenantId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
