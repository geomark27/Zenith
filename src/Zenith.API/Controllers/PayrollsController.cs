using Microsoft.AspNetCore.Mvc;
using Zenith.Application.Interfaces;
using Zenith.Core.Common;
using Zenith.Core.DTOs.Payroll;

namespace Zenith.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PayrollsController(IPayrollService payrollService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<PayrollResponseDto>>>> GetAll(
            [FromQuery] int tenantId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var payrolls    = await payrollService.GetAllAsync(tenantId, startDate, endDate);
            var payrollList = payrolls.ToList();
            
            return Ok(ApiResponse<IEnumerable<PayrollResponseDto>>.SuccessResponse(
                payrollList,
                payrollList.Count,
                "Payrolls retrieved successfully"
            ));
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PayrollResponseDto>>>> GetByEmployeeId(int employeeId, [FromQuery] int tenantId)
        {
            var payrolls = await payrollService.GetByEmployeeIdAsync(employeeId, tenantId);
            
            return Ok(ApiResponse<IEnumerable<PayrollResponseDto>>.SuccessResponse(
                payrolls,
                payrolls.Count(),
                "Payrolls retrieved successfully"
            ));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PayrollResponseDto>>> GetById(int id, [FromQuery] int tenantId)
        {
            var payroll = await payrollService.GetByIdAsync(id, tenantId);
            
            if (payroll == null)
                return NotFound(ApiResponse<PayrollResponseDto>.ErrorResponse("Payroll not found"));

            return Ok(ApiResponse<PayrollResponseDto>.SuccessResponse(payroll, "Payroll retrieved successfully"));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<PayrollResponseDto>>> Create([FromBody] CreatePayrollDto dto)
        {
            int userId = 1;
            var payroll = await payrollService.CreateAsync(dto, userId);
            
            if (payroll == null)
                return BadRequest(ApiResponse<PayrollResponseDto>.ErrorResponse("Failed to create payroll"));

            return CreatedAtAction(
                nameof(GetById), 
                new { id = payroll.Id, tenantId = dto.TenantId }, 
                ApiResponse<PayrollResponseDto>.SuccessResponse(payroll, "Payroll created successfully")
            );
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<PayrollResponseDto>>> Update(int id, [FromBody] UpdatePayrollDto dto, [FromQuery] int tenantId)
        {
            int userId  = 1;
            var payroll = await payrollService.UpdateAsync(id, dto, tenantId, userId);
            
            if (payroll == null)
                return NotFound(ApiResponse<PayrollResponseDto>.ErrorResponse("Payroll not found"));

            return Ok(ApiResponse<PayrollResponseDto>.SuccessResponse(payroll, "Payroll updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id, [FromQuery] int tenantId)
        {
            var result = await payrollService.DeleteAsync(id, tenantId);
            
            if (!result)
                return NotFound(ApiResponse<PayrollResponseDto>.ErrorResponse("Payroll not found"));

            return Ok(ApiResponse<object>.SuccessResponse(null!, "Payroll deleted successfully"));
        }
    }
}
