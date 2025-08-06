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

    public async Task<List<ReportData>> GetAllAsync()
    {
        var reportData = await context.ReportData.ToListAsync();

        return reportData
            .Select(r => r.ToDomain())
            .ToList();
    }
}