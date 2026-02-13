namespace Zenith.Core.DTOs.Employee;

public class EmployeeResponseDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public DateTime HireDate { get; set; }
    public string Position { get; set; } = null!;
    public decimal Salary { get; set; }
    public bool IsActive { get; set; }
    public EmployeeDepartmentDto Department { get; set; } = null!;
}
