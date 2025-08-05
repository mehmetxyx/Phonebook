using ContactManagement.Application.Dtos;
using ContactManagement.Domain.Entities;

namespace ContactManagement.Application.Mappers;

public static class ContactDetailCreateRequestMapper
{
    public static ContactDetail ToDomain(this ContactDetailCreateRequest request)
    {
        return new ContactDetail
        {
            Id = Guid.NewGuid(),
            ContactId = request.ContactId,
            Type = request.Type,
            Value = request.Value
        };
    }

    public static ContactDetailCreateResponse ToContactDetailCreateResponse(this ContactDetail contactDetail)
    {
        return new ContactDetailCreateResponse
        {
            Id = contactDetail.Id,
            ContactId = contactDetail.ContactId,
            Type = contactDetail.Type,
            Value = contactDetail.Value
        };
    }
}