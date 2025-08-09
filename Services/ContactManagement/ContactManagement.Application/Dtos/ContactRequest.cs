namespace ContactManagement.Application.Dtos;

public class ContactRequest
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Company { get; set; }
}