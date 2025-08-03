namespace ContactManagement.Application.Dtos;

public class GetContactResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Company { get; set; }
}