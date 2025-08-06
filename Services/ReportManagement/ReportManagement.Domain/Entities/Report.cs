using ReportManagement.Domain.Enums;

namespace ReportManagement.Domain.Entities;

public class Report
{
    public required Guid Id { get; set; }
    public required DateTimeOffset RequestDate { get; set; }
    public required ReportStatus Status { get; set; }
}