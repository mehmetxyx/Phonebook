using ContactManagement.Application.Dtos;
using Shared.Common;

namespace ContactManagement.Application.Services;

public interface IContactService
{
    Task<Result<ContactCreateResponse>> CreateContactAsync(ContactCreateRequest request);
    Task<Result<ContactGetResponse>> GetContactByIdAsync(Guid contactId);
    Task<Result<List<ContactGetResponse>>> GetContactsAsync();
    Task<Result<bool>> DeleteContactAsync(Guid contactId);
}