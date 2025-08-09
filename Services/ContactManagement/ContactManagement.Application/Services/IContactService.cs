using ContactManagement.Application.Dtos;
using Shared.Common;

namespace ContactManagement.Application.Services;

public interface IContactService
{
    Task<Result<ContactResponse>> CreateContactAsync(ContactRequest request);
    Task<Result<ContactResponse>> GetContactByIdAsync(Guid contactId);
    Task<Result<List<ContactResponse>>> GetContactsAsync();
    Task<Result<bool>> DeleteContactAsync(Guid contactId);
}