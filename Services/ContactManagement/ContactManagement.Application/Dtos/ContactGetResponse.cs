namespace ContactManagement.Application.Dtos;

public class ContactGetResponse
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Company { get; set; }
}