using System.ComponentModel.DataAnnotations;

namespace Zenith.Core.DTOs.Payroll;

public class CreatePayrollDto
{
    [Required]
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

    [Required]
    public int StatusCatalogId { get; set; }

    [Required]
    public int PaymentMethodCatalogId { get; set; }

    [Required]
    public int TenantId { get; set; }
}
