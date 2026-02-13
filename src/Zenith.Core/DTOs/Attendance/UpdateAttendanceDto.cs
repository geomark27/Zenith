namespace Zenith.Core.DTOs.Attendance;

public class UpdateAttendanceDto
{
    public DateTime? CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
    public decimal? WorkedHours { get; set; }
    public int? StatusCatalogId { get; set; }
    public string? Notes { get; set; }
}
