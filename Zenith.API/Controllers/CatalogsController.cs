using Microsoft.AspNetCore.Mvc;
using Zenith.Application.Interfaces;
using Zenith.Core.DTOs.Catalog;

namespace Zenith.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogsController(ICatalogService catalogService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CatalogResponseDto>>> GetAll([FromQuery] int tenantId)
        {
            var results = await catalogService.GetAllAsync(tenantId);
            return Ok(results);
        }

        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<CatalogResponseDto>>> GetByCategory(string category, [FromQuery] int tenantId)
        {
            var results = await catalogService.GetByCategoryAsync(category, tenantId);
            return Ok(results);
        }

        [HttpGet("code/{code}")]
        public async Task<ActionResult<CatalogResponseDto>> GetByCode(string code, [FromQuery] int tenantId)
        {
            var result = await catalogService.GetByCodeAsync(code, tenantId);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CatalogResponseDto>> GetById(int id, [FromQuery] int tenantId)
        {
            var result = await catalogService.GetByIdAsync(id, tenantId);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<CatalogResponseDto>> Create([FromBody] CreateCatalogDto dto)
        {
            int userId = 1;
            var result = await catalogService.CreateAsync(dto, userId);
            if (result == null)
                return BadRequest();

            return CreatedAtAction(nameof(GetById), new { id = result.Id, tenantId = dto.TenantId }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CatalogResponseDto>> Update(int id, [FromBody] UpdateCatalogDto dto, [FromQuery] int tenantId)
        {
            int userId = 1;
            var result = await catalogService.UpdateAsync(id, dto, tenantId, userId);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] int tenantId)
        {
            var result = await catalogService.DeleteAsync(id, tenantId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
