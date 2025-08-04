namespace ContactManagement.Infrastructure.Data.Entities;

public class ContactEntity
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string Surname { get; set; }
    public string Company { get; set; }
}