namespace Zenith.Core.DTOs.Payroll;

public class PayrollResponseDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = null!;
    public DateTime PayPeriodStart { get; set; }
    public DateTime PayPeriodEnd { get; set; }
    public DateTime PaymentDate { get; set; }
    public decimal BaseSalary { get; set; }
    public decimal? Bonuses { get; set; }
    public decimal? OvertimePay { get; set; }
    public decimal? Deductions { get; set; }
    public decimal NetPay { get; set; }
    public int StatusCatalogId { get; set; }
    public string StatusName { get; set; } = null!;
    public int PaymentMethodCatalogId { get; set; }
    public string PaymentMethodName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
