using System.ComponentModel.DataAnnotations;

namespace Zenith.Core.DTOs.Employee
{
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
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = null!; // Extra info
    }
    
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
    
    public class UpdateEmployeeDto
    {
        [Required]
        public required string FirstName { get; set; }
        [Required]
        public required string LastName { get; set; }
        [Required]
        public required string Phone { get; set; }
        public int DepartmentId { get; set; }
        [Required]
        public required string Position { get; set; }
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
    }
}