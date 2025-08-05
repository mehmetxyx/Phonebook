using ContactManagement.Domain.Enums;

namespace ContactManagement.Application.Dtos;

public class ContactDetailCreateResponse
{
    public required Guid Id { get; set; }
    public required Guid ContactId { get; set; }
    public required InformationType Type { get; set; }
    public required string Value { get; set; }
}