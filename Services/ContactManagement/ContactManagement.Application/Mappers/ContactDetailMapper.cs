using ContactManagement.Application.Dtos;
using ContactManagement.Domain.Entities;

namespace ContactManagement.Application.Mappers;
public static class ContactDetailMapper
{
    public static ContactDetailResponse ToContactDetailResponse(this ContactDetail contactDetail)
    {
        return new ContactDetailResponse
        {
            Id = contactDetail.Id,
            ContactId = contactDetail.ContactId,
            Type = contactDetail.Type,
            Value = contactDetail.Value
        };
    }

    public static ContactDetail ToDomain(this ContactDetailRequest request, Guid contactId)
    {
        return new ContactDetail
        {
            Id = Guid.NewGuid(),
            ContactId = contactId,
            Type = request.Type,
            Value = request.Value
        };
    }
}
