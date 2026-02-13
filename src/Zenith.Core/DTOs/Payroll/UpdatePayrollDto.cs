namespace Zenith.Core.DTOs.Payroll;

public class UpdatePayrollDto
{
    public DateTime? PaymentDate { get; set; }
    public decimal? Bonuses { get; set; }
    public decimal? OvertimePay { get; set; }
    public decimal? Deductions { get; set; }
    public decimal? NetPay { get; set; }
    public int? StatusCatalogId { get; set; }
    public int? PaymentMethodCatalogId { get; set; }
}
