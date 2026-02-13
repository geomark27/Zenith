namespace Zenith.Core.DTOs.Department;

public class UpdateDepartmentDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? ManagerId { get; set; }
}