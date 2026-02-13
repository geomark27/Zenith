using System.ComponentModel.DataAnnotations;

namespace Zenith.Core.DTOs.Department
{
    public class DepartmentResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int? ManagerId { get; set; }
        public string? ManagerName { get; set; }
        public int EmployeeCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }

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

    public class DepartmentEmployeeDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Position { get; set; } = null!;
        public bool IsActive { get; set; }
    }

    public class CreateDepartmentDto
    {
        [Required]
        public required string Name { get; set; }
        
        public string? Description { get; set; }
        
        public int? ManagerId { get; set; }
        
        [Required]
        public int TenantId { get; set; }
    }

    public class UpdateDepartmentDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? ManagerId { get; set; }
    }
}