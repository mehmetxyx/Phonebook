using ReportManagement.Application.Dtos;
using ReportManagement.Domain.Entities;

namespace ReportManagement.Application.Mappers;

public static class ReportDataMapper
{
    public static ReportDataResponse ToReportDataResponse(this ReportData report)
    {
        return new ReportDataResponse
        {
            Id = report.Id,
            ReportId = report.ReportId,
            Location = report.Location,
            ContactCount = report.ContactCount,
            PhoneNumberCount = report.PhoneNumberCount
        };
    }
}