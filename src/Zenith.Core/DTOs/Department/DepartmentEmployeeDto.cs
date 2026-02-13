namespace Zenith.Core.DTOs.Department;

public class DepartmentEmployeeDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Position { get; set; } = null!;
    public bool IsActive { get; set; }
}
