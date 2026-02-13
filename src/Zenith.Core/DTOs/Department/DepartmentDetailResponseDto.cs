namespace Zenith.Core.DTOs.Department;

public class DepartmentDetailResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int? ManagerId { get; set; }
    public string? ManagerName { get; set; }
    public int EmployeeCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<DepartmentEmployeeDto> Employees { get; set; } = [];
    public int EmployeePage { get; set; }
    public int EmployeePageSize { get; set; }
    public int EmployeeTotalPages { get; set; }
}