using Microsoft.AspNetCore.Mvc;
using Zenith.Application.Interfaces;
using Zenith.Core.DTOs.Employee;

namespace Zenith.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController(IEmployeeService employeeService) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeResponseDto>>> GetAll([FromQuery] int tenantId)
        {
            var employees = await employeeService.GetAllAsync(tenantId);
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeResponseDto>> GetById(int id, [FromQuery] int tenantId)
        {
            var employee = await employeeService.GetByIdAsync(id, tenantId);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeResponseDto>> Create([FromBody] CreateEmployeeDto dto)
        {
            // TODO: Obtener userId del token JWT cuando implementemos auth
            int userId = 1;

            var employee = await employeeService.CreateAsync(dto, userId);
            if (employee == null)
                return BadRequest();

            return CreatedAtAction(nameof(GetById), new { id = employee.Id, tenantId = dto.TenantId }, employee);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeResponseDto>> Update(int id, [FromBody] UpdateEmployeeDto dto, [FromQuery] int tenantId)
        {
            // TODO: Obtener userId del token JWT
            int userId = 1;

            var employee = await employeeService.UpdateAsync(id, dto, tenantId, userId);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] int tenantId)
        {
            var result = await employeeService.DeleteAsync(id, tenantId);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}