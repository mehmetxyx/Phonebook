using Shared.Common;
using Shared.Contracts;

namespace ReportManagement.Application.EventHandlers;

public interface IReportDataCreatedEventHandler
{
    Task<Result<bool>> SaveReportDataAsync(ReportDataCreatedEvent reportDataCreatedEvent);
}