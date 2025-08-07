namespace Shared.Contracts;
public class ReportDataCreatedEvent
{
    public Guid ReportId { get; set; }
    public List<ReportDataCreated> Data { get; set; }
}