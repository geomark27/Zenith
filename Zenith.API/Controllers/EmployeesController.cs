using Microsoft.AspNetCore.Mvc;
using Zenith.Application.Interfaces;
using Zenith.Core.Common;
using Zenith.Core.DTOs.Employee;

namespace Zenith.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController(IEmployeeService employeeService) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<EmployeeResponseDto>>>> GetAll([FromQuery] int tenantId)
        {
            var employees       = await employeeService.GetAllAsync(tenantId);
            var employeeList    = employees.ToList();
            return Ok(ApiResponse<IEnumerable<EmployeeResponseDto>>.SuccessResponse(
                employeeList, 
                employeeList.Count, 
                "Employees retrieved successfully"
            ));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<EmployeeResponseDto>>> GetById(int id, [FromQuery] int tenantId)
        {
            var employee = await employeeService.GetByIdAsync(id, tenantId);

            if (employee == null)
                return NotFound(ApiResponse<EmployeeResponseDto>.ErrorResponse("Employee not found"));

            return Ok(ApiResponse<EmployeeResponseDto>.SuccessResponse(employee));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<EmployeeResponseDto>>> Create([FromBody] CreateEmployeeDto dto)
        {
            // TODO: Obtener userId del token JWT cuando implementemos auth
            int userId = 1;

            var employee = await employeeService.CreateAsync(dto, userId);
            if (employee == null)
                return BadRequest();

            return CreatedAtAction(
                nameof(GetById), 
                new { id = employee.Id, tenantId = dto.TenantId }, 
                ApiResponse<EmployeeResponseDto>.SuccessResponse(employee, "Employee created successfully")
            );
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<EmployeeResponseDto>>> Update(int id, [FromBody] UpdateEmployeeDto dto, [FromQuery] int tenantId)
        {
            // TODO: Obtener userId del token JWT
            int userId = 1;

            var employee = await employeeService.UpdateAsync(id, dto, tenantId, userId);

            if (employee == null)
                return NotFound(ApiResponse<EmployeeResponseDto>.ErrorResponse("Employee not found"));

            return Ok(ApiResponse<EmployeeResponseDto>.SuccessResponse(employee, "Employee updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id, [FromQuery] int tenantId)
        {
            var result = await employeeService.DeleteAsync(id, tenantId);

            if (!result)
                return NotFound(ApiResponse<object>.ErrorResponse("Employee not found"));

            return Ok(ApiResponse<object>.SuccessResponse(null!, "Employee deleted successfully"));
        }
    }
}