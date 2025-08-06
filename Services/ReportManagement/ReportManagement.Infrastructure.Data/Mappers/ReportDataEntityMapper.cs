using ReportManagement.Domain.Entities;
using ReportManagement.Infrastructure.Data.Entities;

namespace ReportManagement.Infrastructure.Data.Mappers;

public static class ReportDataEntityMapper
{
    public static ReportDataEntity ToEntity(this ReportData reportData)
    {
        return new ReportDataEntity
        {
            Id = reportData.Id,
            ReportId = reportData.ReportId,
            Location = reportData.Location,
            ContactCount = reportData.ContactCount,
            PhoneNumberCount = reportData.PhoneNumberCount,
        };
    }
    
    public static ReportData ToDomain(this ReportDataEntity entity)
    {
        return new ReportData
        {
            Id = entity.Id,
            ReportId = entity.ReportId,
            Location = entity.Location,
            ContactCount = entity.ContactCount,
            PhoneNumberCount = entity.PhoneNumberCount,
        };
    }
}
