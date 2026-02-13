using Microsoft.AspNetCore.Mvc;
using Zenith.Application.Interfaces;
using Zenith.Core.Common;
using Zenith.Core.DTOs.Department;

namespace Zenith.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController(IDepartmentService departmentService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<DepartmentResponseDto>>>> GetAll([FromQuery] int tenantId)
        {
            var departments = await departmentService.GetAllAsync(tenantId);
            var departmentList = departments.ToList();
            return Ok(ApiResponse<IEnumerable<DepartmentResponseDto>>.SuccessResponse(
                departmentList,
                departmentList.Count,
                "Department retrieved successfully"
            ));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<DepartmentDetailResponseDto>>> GetById(
            int id,
            [FromQuery] int tenantId,
            [FromQuery] int employeePage = 1,
            [FromQuery] int employeePageSize = 25)
        {
            var department = await departmentService.GetByIdAsync(id, tenantId, employeePage, employeePageSize);
            if (department == null)
                return NotFound(ApiResponse<DepartmentDetailResponseDto>.ErrorResponse("Department not found"));

            return Ok(ApiResponse<DepartmentDetailResponseDto>.SuccessResponse(department));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<DepartmentDetailResponseDto>>> Create([FromBody] CreateDepartmentDto dto)
        {
            int userId = 1;
            var department = await departmentService.CreateAsync(dto, userId);
            if (department == null)
                return BadRequest();

            return CreatedAtAction(
                nameof(GetById),
                new { id = department.Id, tenantId = dto.TenantId },
                ApiResponse<DepartmentDetailResponseDto>.SuccessResponse(department, "Department created successfully")
            );
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<DepartmentDetailResponseDto>>> Update(int id, [FromBody] UpdateDepartmentDto dto, [FromQuery] int tenantId)
        {
            int userId = 1;
            var department = await departmentService.UpdateAsync(id, dto, tenantId, userId);
            if (department == null)
                return NotFound(ApiResponse<DepartmentDetailResponseDto>.ErrorResponse("Department not found"));

            return Ok(
                ApiResponse<DepartmentDetailResponseDto>.SuccessResponse(department, "Department updated successfully")
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] int tenantId)
        {
            var result = await departmentService.DeleteAsync(id, tenantId);
            if (!result)
                return NotFound(ApiResponse<DepartmentResponseDto>.ErrorResponse("Department not found"));

            return Ok(ApiResponse<object>.SuccessResponse(null!, "Department deleted successfully"));
        }
    }
}
