namespace ContactManagement.Application.Dtos;

public class ContactCreateResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Company { get; set; }
}