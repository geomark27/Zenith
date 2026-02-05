using System.ComponentModel.DataAnnotations;

namespace Zenith.Core.DTOs.Catalog
{
    public class CatalogResponseDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string Value { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int? ParentId { get; set; }
        public string? ParentValue { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateCatalogDto
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Code { get; set; }
        
        [Required]
        public required string Category { get; set; }
        
        [Required]
        public required string Value { get; set; }
        
        [Required]
        public required string Description { get; set; }
        
        public int? ParentId { get; set; }
        public int Order { get; set; }
        
        [Required]
        public int TenantId { get; set; }
    }

    public class UpdateCatalogDto
    {
        public string? Value { get; set; }
        public string? Description { get; set; }
        public int? Order { get; set; }
        public bool? IsActive { get; set; }
    }
}