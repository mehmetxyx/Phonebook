using ContactManagement.Domain.Entities;
using ContactManagement.Domain.Repositories;
using ContactManagement.Infrastructure.Data.Entities;
using ContactManagement.Infrastructure.Data.Mappers;
using Microsoft.EntityFrameworkCore;

namespace ContactManagement.Infrastructure.Data;

public class ContactRepository: IContactRepository
{
    private ContactManagementDbContext context;

    public ContactRepository(ContactManagementDbContext context)
    {
        this.context = context;
    }

    public async Task AddAsync(Contact contact)
    {
        ContactEntity entity = contact.ToEntity();

        await context.Contacts.AddAsync(entity);
    }

    public async Task<List<Contact>> GetAllAsync()
    {
        var contacts = await context.Contacts.ToListAsync();
        return contacts.Select(c => c.ToDomain()).ToList();
    }

    public async Task<Contact?> GetByIdAsync(Guid contactId)
    {
        var entity = await context.Contacts
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == contactId);

        if (entity is null)
            return null;

        return entity.ToDomain();
    }

    public async Task DeleteAsync(Contact contact)
    {
        var entity = await context.Contacts.FirstOrDefaultAsync(c => c.Id == contact.Id);
        if(entity is null)
            throw new KeyNotFoundException($"Contact with ID {contact.Id} not found");
        
        context.Contacts.Remove(entity);
    }
}