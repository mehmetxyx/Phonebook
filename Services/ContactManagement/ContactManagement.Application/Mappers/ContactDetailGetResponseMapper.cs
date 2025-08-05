using ContactManagement.Application.Dtos;
using ContactManagement.Domain.Entities;

namespace ContactManagement.Application.Mappers;

public static class ContactDetailGetResponseMapper
{
    public static ContactDetailGetResponse ToContactDetailGetResponse(this ContactDetail contactDetail)
    {
        return new ContactDetailGetResponse
        {
            Id = contactDetail.Id,
            ContactId = contactDetail.ContactId,
            Type = contactDetail.Type,
            Value = contactDetail.Value
        };
    }
}
