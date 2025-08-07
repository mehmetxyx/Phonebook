using Microsoft.Extensions.Logging;
using ReportManagement.Application.Interfaces;
using ReportManagement.Application.Mappers;
using ReportManagement.Domain.Repositories;
using Shared.Common;
using Shared.Contracts;

namespace ReportManagement.Application.EventHandlers;
public class ReportDataCreatedEventHandler : IReportDataCreatedEventHandler
{
    private ILogger<ReportDataCreatedEventHandler> logger;
    private readonly IUnitOfWork unitOfWork;
    private IReportDataRepository reportDataRepository;

    public ReportDataCreatedEventHandler(ILogger<ReportDataCreatedEventHandler> logger, IUnitOfWork unitOfWork, IReportDataRepository reportDataRepository)
    {
        this.logger = logger;
        this.unitOfWork = unitOfWork;
        this.reportDataRepository = reportDataRepository;
    }

    public async Task<Result<bool>> SaveReportDataAsync(ReportDataCreatedEvent reportDataCreatedEvent)
    {
        try
        {
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
