using Microsoft.AspNetCore.Mvc;
using Zenith.Application.Interfaces;
using Zenith.Core.DTOs.Department;

namespace Zenith.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentResponseDto>>> GetAll([FromQuery] int tenantId)
        {
            var departments = await _departmentService.GetAllAsync(tenantId);
            return Ok(departments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentResponseDto>> GetById(int id, [FromQuery] int tenantId)
        {
            var department = await _departmentService.GetByIdAsync(id, tenantId);
            if (department == null)
                return NotFound();

            return Ok(department);
        }

        [HttpPost]
        public async Task<ActionResult<DepartmentResponseDto>> Create([FromBody] CreateDepartmentDto dto)
        {
            int userId = 1;
            var department = await _departmentService.CreateAsync(dto, userId);
            if (department == null)
                return BadRequest();

            return CreatedAtAction(nameof(GetById), new { id = department.Id, tenantId = dto.TenantId }, department);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DepartmentResponseDto>> Update(int id, [FromBody] UpdateDepartmentDto dto, [FromQuery] int tenantId)
        {
            int userId = 1;
            var department = await _departmentService.UpdateAsync(id, dto, tenantId, userId);
            if (department == null)
                return NotFound();

            return Ok(department);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] int tenantId)
        {
            var result = await _departmentService.DeleteAsync(id, tenantId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
