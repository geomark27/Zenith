using System.ComponentModel.DataAnnotations;

namespace Zenith.Core.DTOs.Employee;

public class CreateEmployeeDto
{
    [Required]
    public required string FirstName { get; set; }
    [Required]
    public required string LastName { get; set; }
    [Required]
    public required string Email { get; set; }
    [Required]
    public required string Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime HireDate { get; set; }
    public int DepartmentId { get; set; }
    [Required]
    public required string Position { get; set; }
    public decimal Salary { get; set; }
    public int TenantId { get; set; }
}
