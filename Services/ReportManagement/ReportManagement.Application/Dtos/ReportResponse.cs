using ReportManagement.Domain.Enums;

namespace ReportManagement.Application.Dtos;

public class ReportResponse
{
    public Guid Id { get; set; }
    public DateTimeOffset RequestDate { get; set; }
    public ReportStatus Status { get; set; }
}