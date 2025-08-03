using ContactManagement.Application.Dtos;
using Shared.Common;

namespace ContactManagement.Application.Services;

public interface IContactService
{
    Result<ContactCreateResponse> CreateContactAsync(ContactCreateRequest request);
    Result<GetContactResponse> GetContactAsync(Guid contactId);
    Result<List<GetContactResponse>> GetContactsAsync();
    Result<bool> DeleteContactAsync(Guid contactId);
}