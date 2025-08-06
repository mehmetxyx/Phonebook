using ReportManagement.Application.Dtos;
using Shared.Common;

namespace ReportManagement.Application.Services;

public interface IReportDataService
{
    Task<Result<List<ReportDataResponse>>> GetAllReportData();
}