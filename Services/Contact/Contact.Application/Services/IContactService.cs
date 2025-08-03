using Shared.Common;

namespace Contact.Application;

public interface IContactService
{
    Result<ContactCreateResponse> CreateContactAsync(ContactCreateRequest request);
    Result<GetContactResponse> GetContactAsync(Guid contactId);
    Result<List<GetContactResponse>> GetContactsAsync();
    Result<bool> DeleteContactAsync(Guid contactId);
}