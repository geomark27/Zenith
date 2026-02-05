using System.ComponentModel.DataAnnotations;

namespace Zenith.Core.Entities 
{
    public class Payroll 
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }

        [Required]
        public required DateTime PayPeriodStart { get; set; }
        [Required]
        public required DateTime PayPeriodEnd { get; set; }
        [Required]
        public required DateTime PaymentDate { get; set; }
        [Required]
        public required decimal BaseSalary { get; set; }
        public decimal? Bonuses { get; set; }
        public decimal? OvertimePay { get; set; }
        public decimal? Deductions { get; set; }

        [Required]
        public required decimal NetPay { get; set; }
        
        public int StatusCatalogId { get; set; }
        [Required]
        public Catalog StatusCatalog { get; set; } = null!; // "Paid", "Pending", etc.
        
        public int PaymentMethodCatalogId { get; set; }
        [Required]
        public Catalog PaymentMethodCatalog { get; set; } = null!;
        
        [Required]
        public int TenantId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Employee Employee { get; set; } = null!;

        [Required]
        public Tenant Tenant { get; set; } = null!;
        
        public int? CreatedById { get; set; }
        public int? UpdatedById { get; set; }

        public User? CreatedBy { get; set; }
        public User? UpdatedBy { get; set; }
    }

}