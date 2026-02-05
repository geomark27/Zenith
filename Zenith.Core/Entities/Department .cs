using System.ComponentModel.DataAnnotations;

namespace Zenith.Core.Entities
{
    public class Department
    {
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public int? ManagerId { get; set; }

        public int TenantId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
        [Required]
        public Tenant Tenant { get; set; } = null!;
        
        public int? CreatedById { get; set; }
        public int? UpdatedById { get; set; }

        public User? CreatedBy { get; set; }
        public User? UpdatedBy { get; set; }
    }
}