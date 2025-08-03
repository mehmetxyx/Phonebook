namespace Contact.Application.Dtos;
using Contact.Domain.DomainEntities;

public static class ContactCreateRequestMapper
{
    public static ContactDomainEntity ToDomain(this ContactCreateRequest request)
    {
        return new ContactDomainEntity
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Surname = request.Surname,
            Company = request.Company,
            PhoneNumber = request.PhoneNumber
        };
    }
}