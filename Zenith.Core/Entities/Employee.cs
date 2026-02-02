using System.ComponentModel.DataAnnotations;

namespace Zenith.Core.Entities
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        [Required]
        public required string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime HireDate { get; set; }

        [Required]
        public required string Position { get; set; }

        [Required]
        public required decimal Salary { get; set; }
        public bool IsActive { get; set; }

        public int DepartmentId { get; set; }
        public int TenantId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        [Required]
        public Department Department { get; set; } = null!;

        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
        public ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();
        [Required]
        public Tenant Tenant { get; set; } = null!;

        public override string ToString()
        {
            return $"{FirstName} {LastName} - {Position}";
        }
    }
}