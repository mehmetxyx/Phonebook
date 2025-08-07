using Microsoft.Extensions.Logging;
using ReportManagement.Application.Dtos;
using ReportManagement.Application.Interfaces;
using ReportManagement.Application.Mappers;
using ReportManagement.Domain.Entities;
using ReportManagement.Domain.Enums;
using ReportManagement.Domain.Repositories;
using Shared.Application.Messaging;
using Shared.Common;
using Shared.Contracts;

namespace ReportManagement.Application.Services;
public class ReportService: IReportService
{
    private readonly ILogger<ReportService> logger;
    private readonly IUnitOfWork unitOfWork;
    private IReportRepository reportRepository;
    private readonly IEventPublisher eventPublisher;

    public ReportService(ILogger<ReportService> logger, IUnitOfWork unitOfWork, IReportRepository reportRepository, IEventPublisher eventPublisher)
    {
        this.logger = logger;
        this.unitOfWork = unitOfWork;
        this.reportRepository = reportRepository;
        this.eventPublisher = eventPublisher;
    }

    public async Task<Result<ReportResponse>> CreateReportAsync()
    {
        try
        {
            var report = new Report
            {
                Id = Guid.NewGuid(),
                RequestDate = DateTimeOffset.UtcNow,
                Status = ReportStatus.Pending,
            };

            await reportRepository.AddAsync(report);
            await unitOfWork.SaveAsync();

            await eventPublisher.PublishAsync(new ReportDataRequestedEvent
            {
                ReportId = report.Id,
                RequestDate = report.RequestDate
            });

            return Result<ReportResponse>.Success(report.ToReportResponse());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating report");
        }

        return Result<ReportResponse>.Failure("Failed to create report");
    }

    public async Task<Result<List<ReportResponse>>> GetAllReportsAsync()
    {
        try
        {
            var reports = await reportRepository.GetAllAsync();
            if (reports == null || !reports.Any())
                return Result<List<ReportResponse>>.Success(new List<ReportResponse>());

            var reportResponses = reports.Select(r => r.ToReportResponse()).ToList();
            
            return Result<List<ReportResponse>>.Success(reportResponses);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving all reports");
        }

        return Result<List<ReportResponse>>.Failure("Failed to retrieve reports");
    }

    public async Task<Result<ReportResponse>> GetReportByIdAsync(Guid reportId)
    {
        try
        {
            var report = await reportRepository.GetByIdAsync(reportId);

            if (report == null)
                return Result<ReportResponse>.Failure($"Report with ID {reportId} not found.");
            
            return Result<ReportResponse>.Success(report.ToReportResponse());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving report by {reportId}", reportId);
        }

        return Result<ReportResponse>.Failure($"Failed to retrieve report with ID {reportId}.");
    }
}