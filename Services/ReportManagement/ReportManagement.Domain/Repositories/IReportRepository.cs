
using ReportManagement.Domain.Entities;

namespace ReportManagement.Domain.Repositories;

public interface IReportRepository
{
    Task AddAsync(Report report);
    Task<List<Report>> GetAllAsync();
    Task<Report?> GetByIdAsync(Guid reportId);
}