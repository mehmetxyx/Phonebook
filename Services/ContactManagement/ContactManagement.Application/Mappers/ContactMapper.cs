using ContactManagement.Application.Dtos;
using ContactManagement.Domain.Entities;

namespace ContactManagement.Application.Mappers;
public static class ContactMapper
{
    public static ContactResponse ToGetContactResponse(this Contact contact)
    {
        return new ContactResponse
        {
            Id = contact.Id,
            Name = contact.Name,
            Surname = contact.Surname,
            Company = contact.Company
        };
    }

    public static ContactResponse ToContactResponse(this Contact contact)
    {
        return new ContactResponse
        {
            Id = contact.Id,
            Name = contact.Name,
            Surname = contact.Surname,
            Company = contact.Company
        };
    }

    public static Contact ToDomain(this ContactRequest request)
    {
        return new Contact
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Surname = request.Surname,
            Company = request.Company
        };
    }
}
