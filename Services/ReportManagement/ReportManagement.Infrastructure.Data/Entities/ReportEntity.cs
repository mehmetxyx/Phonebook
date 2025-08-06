using ReportManagement.Domain.Enums;

namespace ReportManagement.Infrastructure.Data.Entities;

public class ReportEntity
{
    public required Guid Id { get; set; }
    public required DateTimeOffset RequestDate { get; set; }
    public required ReportStatus Status { get; set; }
    public List<ReportDataEntity>? ReportData { get; set; }
}