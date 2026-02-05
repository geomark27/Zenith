using Microsoft.EntityFrameworkCore;
using Zenith.Core.Entities;

namespace Zenith.Infrastructure.Data
{
    public class ZenithDbContext : DbContext
    {
        public ZenithDbContext(DbContextOptions<ZenithDbContext> options) : base(options) {}

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar relaciones y restricciones
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Tenant)
                .WithMany(t => t.Employees)
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Department>()
                .HasOne(d => d.Tenant)
                .WithMany(t => t.Departments)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Catalog>()
                .HasOne(c => c.Tenant)
                .WithMany(t => t.Catalogs)
                .HasForeignKey(c => c.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configurar relaciones de Attendance con Catalog
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.StatusCatalog)
                .WithMany()
                .HasForeignKey(a => a.StatusCatalogId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configurar relaciones de Payroll con Catalog
            modelBuilder.Entity<Payroll>()
                .HasOne(p => p.StatusCatalog)
                .WithMany()
                .HasForeignKey(p => p.StatusCatalogId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payroll>()
                .HasOne(p => p.PaymentMethodCatalog)
                .WithMany()
                .HasForeignKey(p => p.PaymentMethodCatalogId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices para performance
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.TenantId);

            modelBuilder.Entity<Attendance>()
                .HasIndex(a => new { a.EmployeeId, a.Date });

            modelBuilder.Entity<Catalog>()
                .HasIndex(c => new { c.Code, c.TenantId })
                .IsUnique();

            modelBuilder.Entity<Attendance>()
                .Property(a => a.WorkedHours)
                .HasPrecision(5, 2); // 999.99

            modelBuilder.Entity<Employee>()
                .Property(e => e.Salary)
                .HasPrecision(10, 2); // 99999999.99

            modelBuilder.Entity<Payroll>()
                .Property(p => p.BaseSalary)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Payroll>()
                .Property(p => p.Bonuses)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Payroll>()
                .Property(p => p.OvertimePay)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Payroll>()
                .Property(p => p.Deductions)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Payroll>()
                .Property(p => p.NetPay)
                .HasPrecision(10, 2);
                
            modelBuilder.Entity<User>()
                .HasIndex(u => new { u.Email, u.TenantId })
                .IsUnique();

            modelBuilder.Entity<Tenant>().HasData(new Tenant
            {
                Id = 1,
                Name = "AZENTIC SYS",
                Subdomain = "azenticsys.com",
                Email = "azentic@sys.com",
                Phone = "0968319032",
                Address = "GUAYAQUIL",
                TaxId = "0953331675001",
                IsActive = true,
                CreatedAt = new DateTime(2026, 2, 5, 17, 8, 31),
                UpdatedAt = new DateTime(2026, 2, 5, 17, 8, 34)
            });

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Email = "admin@azenticsys.com",
                PasswordHash = "admin123.",
                FirstName = "Admin",
                LastName = "User",
                Role = "ADMIN",
                IsActive = true,
                TenantId = 1,
                CreatedAt = new DateTime(2026, 2, 5, 17, 8, 31),
                UpdatedAt = new DateTime(2026, 2, 5, 17, 8, 34)
            });
        }
    }

}
