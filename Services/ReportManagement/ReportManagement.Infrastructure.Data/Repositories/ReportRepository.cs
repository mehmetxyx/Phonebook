using Microsoft.EntityFrameworkCore;
using ReportManagement.Domain.Entities;
using ReportManagement.Domain.Repositories;
using ReportManagement.Infrastructure.Data.Mappers;

namespace ReportManagement.Infrastructure.Data.Repositories;

public class ReportRepository : IReportRepository
{
    private ReportManagementDbContext context;

    public ReportRepository(ReportManagementDbContext context)
    {
        this.context = context;
    }

    public async Task AddAsync(Report report)
    {
        var entity = report.ToEntity();

        await context.Reports.AddAsync(entity);
    }

    public async Task<List<Report>> GetAllAsync()
    {
        var reports = await context.Reports.ToListAsync();

        return reports
            .Select(r => r.ToDomain())
            .ToList();
    }

    public async Task<Report?> GetByIdAsync(Guid reportId)
    {
        var entity = await context.Reports
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == reportId);

        if(entity is null)
            return null;

        return entity.ToDomain();
    }
}