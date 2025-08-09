using Microsoft.Extensions.Logging;
using ReportManagement.Application.Interfaces;
using ReportManagement.Application.Mappers;
using ReportManagement.Domain.Enums;
using ReportManagement.Domain.Repositories;
using Shared.Common;
using Shared.Contracts;

namespace ReportManagement.Application.EventHandlers;
public class ReportDataCreatedEventHandler : IReportDataCreatedEventHandler
{
    private ILogger<ReportDataCreatedEventHandler> logger;
    private readonly IUnitOfWork unitOfWork;
    private readonly IReportRepository reportRepository;
    private IReportDataRepository reportDataRepository;

    public ReportDataCreatedEventHandler(ILogger<ReportDataCreatedEventHandler> logger, IUnitOfWork unitOfWork, IReportRepository reportRepository, IReportDataRepository reportDataRepository)
    {
        this.logger = logger;
        this.unitOfWork = unitOfWork;
        this.reportRepository = reportRepository;
        this.reportDataRepository = reportDataRepository;
    }

    public async Task<Result<bool>> SaveReportDataAsync(ReportDataCreatedEvent reportDataCreatedEvent)
    {
        try
        {
            var report = await reportRepository.GetByIdAsync(reportDataCreatedEvent.ReportId);
            if (report == null)
            {
                logger.LogError("Report not found for ReportId {ReportId}", reportDataCreatedEvent.ReportId);
                return Result<bool>.Failure($"Report with ID {reportDataCreatedEvent.ReportId} not found.");
            }

            report.Status = ReportStatus.Completed;

            reportRepository.Update(report);

            await reportDataRepository.SaveReportDataAsync(reportDataCreatedEvent.ToReportData());

            await unitOfWork.SaveAsync();

            logger.LogInformation("Report data saved successfully: ReportId {ReportId}", reportDataCreatedEvent.ReportId);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unable to save report data. error {error}", ex.Message);
        }

        return Result<bool>.Failure("Failed to save report data");
    }
}
