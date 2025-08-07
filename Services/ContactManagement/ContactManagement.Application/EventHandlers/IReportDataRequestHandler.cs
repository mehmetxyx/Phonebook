using Shared.Contracts;

namespace ContactManagement.Application.EventHandlers;
public interface IReportDataRequestHandler
{
    Task<ReportDataCreatedEvent> GenerateReportData(ReportDataRequestedEvent message);
}
