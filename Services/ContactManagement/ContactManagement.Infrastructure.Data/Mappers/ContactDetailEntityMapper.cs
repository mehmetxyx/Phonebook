using ContactManagement.Domain.Entities;
using ContactManagement.Infrastructure.Data.Entities;

namespace ContactManagement.Infrastructure.Data.Mappers;

public static class ContactDetailEntityMapper
{
    public static ContactDetailEntity ToEntity(this ContactDetail contact)
    {
        return new ContactDetailEntity
        {
            Id = contact.Id,
            ContactId = contact.ContactId,
            Type = contact.Type,
            Value = contact.Value,
        };
    }
    public static ContactDetail ToDomain(this ContactDetailEntity entity)
    {
        return new ContactDetail
        {
            Id = entity.Id,
            ContactId = entity.ContactId,
            Type = entity.Type,
            Value = entity.Value,
        };
    }
}
