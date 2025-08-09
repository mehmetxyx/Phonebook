using ContactManagement.Application.Dtos;
using ContactManagement.Domain.Entities;

namespace ContactManagement.Application.Mappers;
public static class ContactMapper
{
    public static ContactGetResponse ToGetContactResponse(this Contact contact)
    {
        return new ContactGetResponse
        {
            Id = contact.Id,
            Name = contact.Name,
            Surname = contact.Surname,
            Company = contact.Company
        };
    }

    public static ContactCreateResponse ToContactCreateResponse(this Contact contact)
    {
        return new ContactCreateResponse
        {
            Id = contact.Id,
            Name = contact.Name,
            Surname = contact.Surname,
            Company = contact.Company
        };
    }

    public static Contact ToDomain(this ContactCreateRequest request)
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
