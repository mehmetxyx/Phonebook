using ContactManagement.Domain.Entities;

namespace ContactManagement.Domain.Repositories;

public interface IContactDetailRepository
{
    Task AddAsync(ContactDetail contactDetail);
    Task<List<ContactDetail>> GetAllAsync(Guid contactId);
    Task<ContactDetail?> GetByIdAsync(Guid contactId, Guid contactDetailId);
    void Delete(ContactDetail contactDetail);
}