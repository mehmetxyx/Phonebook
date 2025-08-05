
using ContactManagement.Domain.Enums;

namespace ContactManagement.Domain.Entities;

public class ContactDetail
{
    public required Guid Id { get; set; }
    public required Guid ContactId { get; set; }
    public required InformationType Type { get; set; }
    public required string Value { get; set; }
}