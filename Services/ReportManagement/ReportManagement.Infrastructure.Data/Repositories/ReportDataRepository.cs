using Microsoft.EntityFrameworkCore;
using ReportManagement.Domain.Entities;
using ReportManagement.Domain.Repositories;
using ReportManagement.Infrastructure.Data.Mappers;

namespace ReportManagement.Infrastructure.Data.Repositories;

public class ReportDataRepository : IReportDataRepository
{
    private ReportManagementDbContext context;

    public ReportDataRepository(ReportManagementDbContext context)
    {
        this.context = context;
    }

    public async Task<List<ReportData>> GetAllAsync(Guid reportId)
    {
        var reportData = await context.ReportData
            .Where(r => r.ReportId == reportId)
            .ToListAsync();

        return reportData
            .Select(r => r.ToDomain())
            .ToList();
    }

    public async Task SaveReportDataAsync(List<ReportData> reportData)
    {
        var reportDataEntities = reportData
            .Select(r => r.ToEntity())
            .ToList();

        await context.ReportData.AddRangeAsync(reportDataEntities);
    }
}