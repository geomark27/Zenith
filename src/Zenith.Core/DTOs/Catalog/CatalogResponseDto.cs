namespace Zenith.Core.DTOs.Catalog;

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