using System.ComponentModel.DataAnnotations;

namespace Zenith.Core.DTOs.Attendance;

public class CreateAttendanceDto
{
    [Required]
    public int EmployeeId { get; set; }

    [Required]
    public required DateTime Date { get; set; }

    public DateTime? CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
    public decimal? WorkedHours { get; set; }

    [Required]
    public int StatusCatalogId { get; set; }

    public string? Notes { get; set; }

    [Required]
    public int TenantId { get; set; }
}
