using ReportManagement.Domain.Entities;

namespace ReportManagement.Domain.Repositories;

public interface IReportDataRepository
{
    Task<List<ReportData>> GetAllAsync();
    Task SaveReportDataAsync(List<ReportData> reportData);
}