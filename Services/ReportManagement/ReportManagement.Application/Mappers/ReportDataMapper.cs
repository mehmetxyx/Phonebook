using ReportManagement.Application.Dtos;
using ReportManagement.Domain.Entities;
using Shared.Contracts;

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

    public static List<ReportData> ToReportData(this ReportDataCreatedEvent reportDataCreatedEvent)
    { 
        return reportDataCreatedEvent.Data
            .Select(r => new ReportData {
                    Id = Guid.NewGuid(),
                    ReportId = reportDataCreatedEvent.ReportId,
                    Location = r.Location,
                    ContactCount = r.ContactCount,
                    PhoneNumberCount = r.PhoneNumberCount
             })
            .ToList();
    }
}