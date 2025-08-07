using ContactManagement.Application.Dtos;
using ContactManagement.Application.Mappers;
using Shared.Contracts;

namespace ContactManagement.Application.EventHandlers;
public class ReportDataRequestHandler : IReportDataRequestHandler
{
    public Task<ReportDataCreatedEvent> GenerateReportData(ReportDataRequestedEvent reportDataRequestedEvent)
    {
        var contactReportData = new List<ContactReportData>
        {
            new ContactReportData
            {
                Location = "London",
                ContactCount = 23,
                PhoneNumberCount = 44,
            },
            new ContactReportData
            {
                Location = "Paris",
                ContactCount = 33,
                PhoneNumberCount = 54,
            }
        };

        var reportDataCreatedEvent = ReportDataMapper.ToReportDataCreatedEvent(
        reportDataRequestedEvent.ReportId,
        reportDataRequestedEvent.RequestDate,
        contactReportData);

        return Task.FromResult(reportDataCreatedEvent);
    }
}
