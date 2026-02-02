using System.ComponentModel.DataAnnotations;

namespace Zenith.Core.Entities 
{ 
    public class Catalog 
    {
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Code { get; set; } // "PAYMENT_STATUS", "PAYMENT_METHOD", "ATTENDANCE_STATUS"
        [Required]
        public required string Category { get; set; } // Para agrupar: "Payment", "Attendance", "Employee"
        [Required]
        public required string Value { get; set; } // El texto: "Paid", "Pending", "Bank Transfer"
        [Required]
        public required string Description { get; set; }

        public int? ParentId { get; set; } // Para jerarquías
        public int Order { get; set; } // Para ordenar en UI
        public bool IsActive { get; set; }

        // Multi-tenant
        public int TenantId { get; set; }

        // Navegación para jerarquía
        public Catalog? Parent { get; set; }
        public ICollection<Catalog> Children { get; set; } = new List<Catalog>();

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        [Required]
        public Tenant Tenant { get; set; } = null!;
    }
}