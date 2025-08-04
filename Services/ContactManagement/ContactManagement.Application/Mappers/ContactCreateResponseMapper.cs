using ContactManagement.Domain.Entities;

namespace ContactManagement.Application.Dtos;

public static class ContactCreateResponseMapper
{
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
}