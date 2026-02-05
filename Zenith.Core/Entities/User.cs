using System.ComponentModel.DataAnnotations;

namespace Zenith.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        public required string Email { get; set; }
        
        [Required]
        public required string PasswordHash { get; set; }
        
        [Required]
        public required string FirstName { get; set; }
        
        [Required]
        public required string LastName { get; set; }
        
        [Required]
        public required string Role { get; set; } // Admin, Manager, Employee
        
        public bool IsActive { get; set; }
        
        public int TenantId { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        public Tenant Tenant { get; set; } = null!;
    }
}