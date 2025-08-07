namespace Shared.Contracts;
public class ReportDataRequestedEvent
{
    public Guid ReportId { get; set; }
    public DateTimeOffset RequestDate { get; set; }
}