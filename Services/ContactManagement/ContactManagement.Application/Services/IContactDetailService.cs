using ContactManagement.Application.Dtos;
using Shared.Common;

namespace ContactManagement.Application.Services;

public interface IContactDetailService
{
    Task<Result<ContactDetailCreateResponse>> CreateContactDetailAsync(Guid contactId, ContactDetailCreateRequest request);
    Task<Result<bool>> DeleteContactDetailAsync(Guid contactId, Guid contactDetailId);
    Task<Result<List<ContactDetailGetResponse>>> GetAllContactDetailsAsync(Guid contactId);
    Task<Result<ContactDetailGetResponse>> GetContactDetailByIdAsync(Guid contactId, Guid contactDetailId);
}