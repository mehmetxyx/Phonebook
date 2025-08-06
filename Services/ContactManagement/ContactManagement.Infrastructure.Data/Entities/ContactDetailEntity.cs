using ContactManagement.Domain.Enums;

namespace ContactManagement.Infrastructure.Data.Entities;

public class ContactDetailEntity
{
    public required Guid Id { get; set; }
    public required Guid ContactId { get; set; }
    public required InformationType Type { get; set; }
    public required string Value { get; set; }
    public ContactEntity? Contact { get; set; }
}