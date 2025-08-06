using ReportManagement.Application.Dtos;
using Shared.Common;
namespace ReportManagement.Application.Services;

public interface IReportService
{
    Task<Result<ReportResponse>> CreateReportAsync();
    Task<Result<List<ReportResponse>>> GetAllReportsAsync();
    Task<Result<ReportResponse>> GetReportByIdAsync(Guid reportId);
}