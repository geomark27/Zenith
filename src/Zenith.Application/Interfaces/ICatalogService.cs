using Zenith.Core.DTOs.Catalog;

namespace Zenith.Application.Interfaces
{
    public interface ICatalogService
    {
        Task<IEnumerable<CatalogResponseDto>> GetAllAsync(int tenantId);
        Task<IEnumerable<CatalogResponseDto>> GetByCategoryAsync(string category, int tenantId);
        Task<CatalogResponseDto?> GetByIdAsync(int id, int tenantId);
        Task<CatalogResponseDto?> GetByCodeAsync(string code, int tenantId);
        Task<CatalogResponseDto> CreateAsync(CreateCatalogDto dto, int userId);
        Task<CatalogResponseDto?> UpdateAsync(int id, UpdateCatalogDto dto, int tenantId, int userId);
        Task<bool> DeleteAsync(int id, int tenantId);
    }
}