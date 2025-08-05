using ContactManagement.Domain.Enums;

namespace ContactManagement.Application.Dtos;

public class ContactDetailCreateRequest
{
    public required Guid ContactId { get; set; }
    public required InformationType Type { get; set; }
    public required string Value { get; set; }
}