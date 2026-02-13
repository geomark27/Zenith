using System.ComponentModel.DataAnnotations;

namespace Zenith.Core.DTOs.Catalog;

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
