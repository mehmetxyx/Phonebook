using Contact.Domain.DomainEntities;

namespace Contact.Application.Dtos;

public static class ContactCreateResponseMapper
{
    public static ContactCreateResponse ToResponse(this ContactDomainEntity contact)
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