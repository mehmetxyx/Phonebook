using ReportManagement.Domain.Entities;
using ReportManagement.Infrastructure.Data.Entities;

namespace ReportManagement.Infrastructure.Data.Mappers;

public static class ReportEntityMapper
{
    public static ReportEntity ToEntity(this Report report)
    {
        return new ReportEntity
        {
            Id = report.Id,
            RequestDate = report.RequestDate,
            Status = report.Status,
        };
    }
    
    public static Report ToDomain(this ReportEntity entity)
    {
        return new Report
        {
            Id = entity.Id,
            RequestDate = entity.RequestDate,
            Status = entity.Status,
        };
    }
}