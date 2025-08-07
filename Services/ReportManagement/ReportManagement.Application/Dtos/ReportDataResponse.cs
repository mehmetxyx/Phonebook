
namespace ReportManagement.Application.Dtos;

public class ReportDataResponse
{
    public Guid Id { get; set; }
    public Guid ReportId { get; set; }
    public required string Location { get; set; }
    public int ContactCount { get; set; }
    public int PhoneNumberCount { get; set; }
}