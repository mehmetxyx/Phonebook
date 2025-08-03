
namespace ContactManagement.Domain.DomainEntities;
public class Contact
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Company { get; set; }
    public string PhoneNumber { get; set; }
    public Guid Id { get; set; }
}
