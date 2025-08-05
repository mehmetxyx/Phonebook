namespace ContactManagement.Domain.Entities;
public class Contact
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Company { get; set; }
}
