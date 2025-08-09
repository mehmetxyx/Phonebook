namespace ContactManagement.Application.Dtos;

public class ContactResponse
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Company { get; set; }
}