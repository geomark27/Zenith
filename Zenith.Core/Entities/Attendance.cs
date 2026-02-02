using System.ComponentModel.DataAnnotations;

namespace Zenith.Core.Entities 
{
    public class Attendance
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }

        [Required]
        public required DateTime CheckInTime { get; set; }
        [Required]
        public required DateTime CheckOutTime { get; set; }
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
    }
}