namespace Zenith.Core.DTOs.Catalog;

public class UpdateCatalogDto
{
    public string? Value { get; set; }
    public string? Description { get; set; }
    public int? Order { get; set; }
    public bool? IsActive { get; set; }
}