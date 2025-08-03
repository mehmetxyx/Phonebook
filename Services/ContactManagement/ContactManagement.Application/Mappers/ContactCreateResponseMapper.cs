using ContactManagement.Domain.DomainEntities;

namespace ContactManagement.Application.Dtos;

public static class ContactCreateResponseMapper
{
    public static ContactCreateResponse ToResponse(this Contact contact)
    {
        return new ContactCreateResponse
        {
            Id = contact.Id,
            Name = contact.Name,
            Surname = contact.Surname,
            Company = contact.Company,
            PhoneNumber = contact.PhoneNumber
        };
    }
}