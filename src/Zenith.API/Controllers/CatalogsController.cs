using Microsoft.AspNetCore.Mvc;
using Zenith.Application.Interfaces;
using Zenith.Core.Common;
using Zenith.Core.DTOs.Catalog;

namespace Zenith.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogsController(ICatalogService catalogService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<CatalogResponseDto>>>> GetAll([FromQuery] int tenantId)
        {
            var catalogs = await catalogService.GetAllAsync(tenantId);
            var catalogList = catalogs.ToList();

            return Ok(ApiResponse<IEnumerable<CatalogResponseDto>>.SuccessResponse(
                catalogList,
                catalogList.Count(),
                "Catalogs retrieved successfully"
            ));
        }

        [HttpGet("category/{category}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<CatalogResponseDto>>>> GetByCategory(string category, [FromQuery] int tenantId)
        {
            var catalogs = await catalogService.GetByCategoryAsync(category, tenantId);
            var catalogList = catalogs.ToList();

            return Ok(ApiResponse<IEnumerable<CatalogResponseDto>>.SuccessResponse(
                catalogList,
                catalogList.Count(),
                "Catalogs retrieved successfully"
            ));
        }

        [HttpGet("code/{code}")]
        public async Task<ActionResult<ApiResponse<CatalogResponseDto>>> GetByCode(string code, [FromQuery] int tenantId)
        {
            var catalog = await catalogService.GetByCodeAsync(code, tenantId);

            if (catalog == null)
                return NotFound(ApiResponse<CatalogResponseDto>.ErrorResponse("Catalog not found"));

            return Ok(ApiResponse<CatalogResponseDto>.SuccessResponse(catalog));

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CatalogResponseDto>>> GetById(int id, [FromQuery] int tenantId)
        {
            var catalog = await catalogService.GetByIdAsync(id, tenantId);

            if (catalog == null)
                return NotFound(ApiResponse<CatalogResponseDto>.ErrorResponse("Catalog not found"));

            return Ok(ApiResponse<CatalogResponseDto>.SuccessResponse(catalog));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<CatalogResponseDto>>> Create([FromBody] CreateCatalogDto dto)
        {
            int userId = 1;
            var catalog = await catalogService.CreateAsync(dto, userId);
            
            if (catalog == null)
                return BadRequest(ApiResponse<CatalogResponseDto>.ErrorResponse("Failed to create catalog"));

            return CreatedAtAction(
                nameof(GetById),
                new { id = catalog.Id, tenantId = dto.TenantId },
                ApiResponse<CatalogResponseDto>.SuccessResponse(catalog, "Catalog created successfully")
            );
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<CatalogResponseDto>>> Update(int id, [FromBody] UpdateCatalogDto dto, [FromQuery] int tenantId)
        {
            int userId  = 1;
            var catalog = await catalogService.UpdateAsync(id, dto, tenantId, userId);
            
            if (catalog == null)
                return NotFound(ApiResponse<CatalogResponseDto>.ErrorResponse("Catalog not found"));

            return Ok(
                ApiResponse<CatalogResponseDto>.SuccessResponse(catalog, "Catalog updated successfully")
            );
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id, [FromQuery] int tenantId)
        {
            var catalogs = await catalogService.DeleteAsync(id, tenantId);
            if (!catalogs)
                return NotFound(ApiResponse<CatalogResponseDto>.ErrorResponse("Catalog not found"));

            return Ok(
                ApiResponse<object>.SuccessResponse(null!, "Catalog deleted successfully")
            );
        }
    }
}
