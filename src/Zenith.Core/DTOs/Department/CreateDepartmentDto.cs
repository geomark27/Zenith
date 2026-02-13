using System.ComponentModel.DataAnnotations;

namespace Zenith.Core.DTOs.Department;

public class CreateDepartmentDto
{
    [Required]
    public required string Name { get; set; }

    public string? Description { get; set; }

    public int? ManagerId { get; set; }

    [Required]
    public int TenantId { get; set; }
}
