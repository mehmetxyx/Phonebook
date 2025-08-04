namespace ContactManagement.Application.Dtos;

public static class GetContactResponseMapper
{
    public static GetContactResponse ToGetContactResponse(this Domain.Entities.Contact contact)
    {
        return new GetContactResponse
        {
            Id = contact.Id,
            Name = contact.Name,
            Surname = contact.Surname,
            Company = contact.Company
        };
    }
}