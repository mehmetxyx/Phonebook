namespace ContactManagement.Application.Dtos;

public class ContactReportData
{
    public required string Location { get; set; }
    public int ContactCount { get; set; }
    public int PhoneNumberCount { get; set; }
}
