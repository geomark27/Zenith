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