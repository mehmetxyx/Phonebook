namespace ContactManagement.Application.Dtos;
using ContactManagement.Domain.DomainEntities;

public static class ContactCreateRequestMapper
{
    public static Contact ToDomain(this ContactCreateRequest request)
    {
        return new Contact
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Surname = request.Surname,
            Company = request.Company,
            PhoneNumber = request.PhoneNumber
        };
    }
}