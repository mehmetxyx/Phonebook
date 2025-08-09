using ContactManagement.Domain.Enums;

namespace ContactManagement.Application.Dtos;

public class ContactDetailRequest
{
    public required InformationType Type { get; set; }
    public required string Value { get; set; }
}