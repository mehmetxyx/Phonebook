using ContactManagement.Domain.Entities;

namespace ContactManagement.Domain.Repositories;

public interface IContactRepository
{
    Task AddAsync(Contact contact);
    Task DeleteAsync(Guid contactId);
    Task DeleteAsync(Contact contact);
    Task<List<Contact>> GetAllAsync();
    Task<Contact?> GetByIdAsync(Guid contactId);
}