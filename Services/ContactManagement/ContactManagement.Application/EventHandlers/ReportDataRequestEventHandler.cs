using ContactManagement.Application.Mappers;
using ContactManagement.Application.Repositories;
using Microsoft.Extensions.Logging;
using Shared.Common;
using Shared.Contracts;

namespace ContactManagement.Application.EventHandlers;
public class ReportDataRequestEventHandler : IReportDataRequestEventHandler
{
    private ILogger<ReportDataRequestEventHandler> logger;
    private IContactReportDataRepository contactReportDataRepository;

    public ReportDataRequestEventHandler(IContactReportDataRepository contactReportDataRepository, ILogger<ReportDataRequestEventHandler> logger)
    {
        this.contactReportDataRepository = contactReportDataRepository;
        this.logger = logger;
    }

    public async Task<Result<ReportDataCreatedEvent>> GenerateReportDataAsync(ReportDataRequestedEvent reportDataRequestedEvent)
    {
        try
        {
            var contactReportData = await contactReportDataRepository.GetContactReportDataAsync();

            var reportDataCreatedEvent = ReportDataMapper.ToReportDataCreatedEvent(
                reportDataRequestedEvent.ReportId,
                reportDataRequestedEvent.RequestDate,
                contactReportData
            );

            return Result<ReportDataCreatedEvent>.Success(reportDataCreatedEvent);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error generating report data for ReportId: {ReportId}", reportDataRequestedEvent.ReportId);
        }

        return Result<ReportDataCreatedEvent>.Failure($"Failed to generate report data for ReportId: {reportDataRequestedEvent.ReportId}.");
    }
}
