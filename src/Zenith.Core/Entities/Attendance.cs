using System.ComponentModel.DataAnnotations;

namespace Zenith.Core.Entities 
{
    public class Attendance
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }

        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public decimal? WorkedHours { get; set; }

        public int StatusCatalogId { get; set; }
        [Required]
        public Catalog StatusCatalog { get; set; } = null!;
        public string? Notes { get; set; }

        [Required]
        public int TenantId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        [Required]
        public Employee Employee { get; set; } = null!;
        [Required]
        public Tenant Tenant { get; set; } = null!;
        
        public int? CreatedById { get; set; }
        public int? UpdatedById { get; set; }

        public User? CreatedBy { get; set; }
        public User? UpdatedBy { get; set; }
    }
}