namespace Shared.Contracts;
public class ReportDataCreated
{
    public required string Location { get; set; }
    public required int ContactCount { get; set; }
    public required int PhoneNumberCount { get; set; }
}