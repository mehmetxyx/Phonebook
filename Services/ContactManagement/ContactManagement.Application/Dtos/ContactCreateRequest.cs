namespace ContactManagement.Application.Dtos;

public class ContactCreateRequest
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Company { get; set; }
    public string PhoneNumber { get; set; }
}