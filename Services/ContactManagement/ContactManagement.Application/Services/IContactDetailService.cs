using ContactManagement.Application.Dtos;
using Shared.Common;

namespace ContactManagement.Application.Services;

public interface IContactDetailService
{
    Task<Result<ContactDetailResponse>> CreateContactDetailAsync(Guid contactId, ContactDetailRequest request);
    Task<Result<bool>> DeleteContactDetailAsync(Guid contactId, Guid contactDetailId);
    Task<Result<List<ContactDetailResponse>>> GetAllContactDetailsAsync(Guid contactId);
    Task<Result<ContactDetailResponse>> GetContactDetailByIdAsync(Guid contactId, Guid contactDetailId);
}