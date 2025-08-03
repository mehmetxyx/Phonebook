namespace ContactManagement.Application.Dtos;

public class ContactCreateResponse
{
    public Guid Id { get; set; }
    public string Name { get; internal set; }
    public string Surname { get; internal set; }
    public string Company { get; internal set; }
    public string PhoneNumber { get; internal set; }
}