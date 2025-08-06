namespace ReportManagement.Domain.Entities;

public class ReportData
{
    public required Guid Id { get; set; }
    public required Guid ReportId { get; set; }
    public required string Location { get; set; }
    public int ContactCount { get; set; }
    public int PhoneNumberCount { get; set; }
}