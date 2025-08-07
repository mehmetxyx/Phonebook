using ContactManagement.Application.Dtos;
using Shared.Contracts;

namespace ContactManagement.Application.Mappers;
public static class ReportDataMapper
{
    public static ReportDataCreatedEvent ToReportDataCreatedEvent(
        Guid reportId,
        DateTimeOffset requestDate,
        IEnumerable<ContactReportData> contactReportData)
    {
        var mappedData = contactReportData.Select(d => new ReportDataCreated
        {
            Location = d.Location,
            ContactCount = d.ContactCount,
            PhoneNumberCount = d.PhoneNumberCount
        }).ToList();

        return new ReportDataCreatedEvent
        {
            ReportId = reportId,
            Data = mappedData
        };
    }
}