using Microsoft.EntityFrameworkCore;
using Zenith.Application.Interfaces;
using Zenith.Core.DTOs.Catalog;
using Zenith.Core.Entities;
using Zenith.Infrastructure.Data;

namespace Zenith.Application.Services
{
    public class CatalogService(ZenithDbContext context) : ICatalogService
    {
        public async Task<IEnumerable<CatalogResponseDto>> GetAllAsync(int tenantId)
        {
            return await context.Catalogs
                .Where(c => c.TenantId == tenantId)
                .Select(c => new CatalogResponseDto
                {
                    Id          = c.Id,
                    Code        = c.Code,
                    Category    = c.Category,
                    Value       = c.Value,
                    Description = c.Description,
                    ParentId    = c.ParentId,
                    ParentValue = c.ParentId != null 
                        ? context.Catalogs
                            .Where(p => p.Id == c.ParentId)
                            .Select(p => p.Value)
                            .FirstOrDefault()
                        : null,
                    Order       = c.Order,
                    IsActive    = c.IsActive
                })
                .OrderBy(c => c.Category)
                .ThenBy(c => c.Order)
                .ToListAsync();
        }

        public async Task<IEnumerable<CatalogResponseDto>> GetByCategoryAsync(string category, int tenantId)
        {
            return await context.Catalogs
                .Where(c => c.Category == category && c.TenantId == tenantId && c.IsActive)
                .Select(c => new CatalogResponseDto
                {
                    Id          = c.Id,
                    Code        = c.Code,
                    Category    = c.Category,
                    Value       = c.Value,
                    Description = c.Description,
                    ParentId    = c.ParentId,
                    ParentValue = c.ParentId != null 
                        ? context.Catalogs
                            .Where(p => p.Id == c.ParentId)
                            .Select(p => p.Value)
                            .FirstOrDefault()
                        : null,
                    Order       = c.Order,
                    IsActive    = c.IsActive
                })
                .OrderBy(c => c.Order)
                .ToListAsync();
        }

        public async Task<CatalogResponseDto?> GetByIdAsync(int id, int tenantId)
        {
            return await context.Catalogs
                .Where(c => c.Id == id && c.TenantId == tenantId)
                .Select(c => new CatalogResponseDto
                {
                    Id          = c.Id,
                    Code        = c.Code,
                    Category    = c.Category,
                    Value       = c.Value,
                    Description = c.Description,
                    ParentId    = c.ParentId,
                    ParentValue = c.ParentId != null 
                        ? context.Catalogs
                            .Where(p => p.Id == c.ParentId)
                            .Select(p => p.Value)
                            .FirstOrDefault()
                        : null,
                    Order       = c.Order,
                    IsActive    = c.IsActive
                })
                .FirstOrDefaultAsync();
        }

        public async Task<CatalogResponseDto?> GetByCodeAsync(string code, int tenantId)
        {
            return await context.Catalogs
                .Where(c => c.Code == code && c.TenantId == tenantId)
                .Select(c => new CatalogResponseDto
                {
                    Id          = c.Id,
                    Code        = c.Code,
                    Category    = c.Category,
                    Value       = c.Value,
                    Description = c.Description,
                    ParentId    = c.ParentId,
                    ParentValue = c.ParentId != null 
                        ? context.Catalogs
                            .Where(p => p.Id == c.ParentId)
                            .Select(p => p.Value)
                            .FirstOrDefault()
                        : null,
                    Order       = c.Order,
                    IsActive    = c.IsActive
                })
                .FirstOrDefaultAsync();
        }

        public async Task<CatalogResponseDto> CreateAsync(CreateCatalogDto dto, int userId)
        {
            var catalog = new Catalog
            {
                Name        = dto.Name,
                Code        = dto.Code,
                Category    = dto.Category,
                Value       = dto.Value,
                Description = dto.Description,
                ParentId    = dto.ParentId,
                Order       = dto.Order,
                IsActive    = true,
                TenantId    = dto.TenantId,
                CreatedAt   = DateTime.UtcNow,
                CreatedById = userId
            };

            context.Catalogs.Add(catalog);
            await context.SaveChangesAsync();

            return (await GetByIdAsync(catalog.Id, dto.TenantId))!;
        }

        public async Task<CatalogResponseDto?> UpdateAsync(int id, UpdateCatalogDto dto, int tenantId, int userId)
        {
            var catalog = await context.Catalogs
                .FirstOrDefaultAsync(c => c.Id == id && c.TenantId == tenantId);

            if (catalog == null)
                return null;

            if (dto.Value != null) catalog.Value = dto.Value;
            if (dto.Description != null) catalog.Description = dto.Description;
            if (dto.Order.HasValue) catalog.Order = dto.Order.Value;
            if (dto.IsActive.HasValue) catalog.IsActive = dto.IsActive.Value;
            
            catalog.UpdatedAt = DateTime.UtcNow;
            catalog.UpdatedById = userId;

            await context.SaveChangesAsync();

            return await GetByIdAsync(id, tenantId);
        }

        public async Task<bool> DeleteAsync(int id, int tenantId)
        {
            var catalog = await context.Catalogs
                .FirstOrDefaultAsync(c => c.Id == id && c.TenantId == tenantId);

            if (catalog == null)
                return false;

            context.Catalogs.Remove(catalog);
            await context.SaveChangesAsync();

            return true;
        }
    }
}