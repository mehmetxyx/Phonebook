namespace ContactManagement.Application.Dtos;

public static class ContactGetResponseMapper
{
    public static ContactGetResponse ToGetContactResponse(this Domain.Entities.Contact contact)
    {
        return new ContactGetResponse
        {
            Id = contact.Id,
            Name = contact.Name,
            Surname = contact.Surname,
            Company = contact.Company
        };
    }
}