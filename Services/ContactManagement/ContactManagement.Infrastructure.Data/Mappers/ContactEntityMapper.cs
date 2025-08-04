using ContactManagement.Domain.Entities;
using ContactManagement.Infrastructure.Data.Entities;

namespace ContactManagement.Infrastructure.Data.Mappers;

public static class ContactEntityMapper
{
    public static ContactEntity ToEntity(this Contact contact)
    {
        return new ContactEntity
        {
            Id = contact.Id,
            Name = contact.Name,
            Surname = contact.Surname,
            Company = contact.Company,
        };
    }
    public static Contact ToDomain(this ContactEntity entity)
    {
        return new Contact
        {
            Id = entity.Id,
            Name = entity.Name,
            Surname = entity.Surname,
            Company = entity.Company,
        };
    }
}