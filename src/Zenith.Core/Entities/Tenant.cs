using System.ComponentModel.DataAnnotations;

namespace Zenith.Core.Entities
{
    public class Tenant
    {
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Subdomain { get; set; }

        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Phone { get; set; }

        [Required]
        public required string Address { get; set; }

        [Required]
        public required string TaxId { get; set; }

        // Configuración básica
        public bool IsActive { get; set; }

        // Auditoría
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navegación - NO usar [Required] ni required en ICollection
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public ICollection<Department> Departments { get; set; } = new List<Department>();
        public ICollection<Catalog> Catalogs { get; set; } = new List<Catalog>();
    }
}