using Microsoft.Extensions.Logging;
using ReportManagement.Application.Dtos;
using ReportManagement.Application.Mappers;
using ReportManagement.Application.Interfaces;
using ReportManagement.Domain.Repositories;
using Shared.Common;

namespace ReportManagement.Application.Services;

public class ReportDataService: IReportDataService
{
    private ILogger<ReportDataService> logger;
    private IUnitOfWork unitOfWork;
    private IReportDataRepository reportDataRepository;

    public ReportDataService(ILogger<ReportDataService> logger, IUnitOfWork unitOfWork, IReportDataRepository reportDataRepository)
    {
        this.logger = logger;
        this.unitOfWork = unitOfWork;
        this.reportDataRepository = reportDataRepository;
    }

    public async Task<Result<List<ReportDataResponse>>> GetAllReportData()
    {
        try
        {
            var reportData = await reportDataRepository.GetAllAsync();
            if (reportData == null || !reportData.Any())
                return Result<List<ReportDataResponse>>.Success("No report data found.");

            var response = reportData.Select(rd => rd.ToReportDataResponse()).ToList();

            return Result<List<ReportDataResponse>>.Success(response, "Report data retrieved successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting all report data.");
        }

        return Result<List<ReportDataResponse>>.Failure("An error occurred while getting all report data.");
    }
}