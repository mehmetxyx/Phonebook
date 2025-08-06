using ReportManagement.Application.Dtos;
using ReportManagement.Domain.Entities;

namespace ReportManagement.Application.Mappers;

public static class ReportMapper
{
    public static ReportResponse ToReportResponse(this Report report)
    {
        return new ReportResponse
        {
            Id = report.Id,
            RequestDate = report.RequestDate,
            Status = report.Status
        };
    }
}