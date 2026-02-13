namespace Zenith.Core.DTOs.Employee;

public class UpdateEmployeeDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public int? DepartmentId { get; set; }
    public string? Position { get; set; }
    public decimal? Salary { get; set; }
    public bool? IsActive { get; set; }
}
