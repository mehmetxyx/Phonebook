using Shared.Common;
using Shared.Contracts;

namespace ContactManagement.Application.EventHandlers;
public interface IReportDataRequestEventHandler
{
    Task<Result<ReportDataCreatedEvent>> GenerateReportDataAsync(ReportDataRequestedEvent reportDataRequestedEvent);
}
