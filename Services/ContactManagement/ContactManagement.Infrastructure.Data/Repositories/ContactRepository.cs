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
        var contacts = await context.Contacts
            .ToListAsync();
        
        return contacts
            .Select(c => c.ToDomain())
            .ToList();
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

    public void Delete(Contact contact)
    {
        var trackedEntity = context.ChangeTracker.Entries<ContactEntity>()
            .FirstOrDefault(e => e.Entity.Id == contact.Id);

        if(trackedEntity is not null)
        {
            trackedEntity.State = EntityState.Deleted;
            return;
        }

        var entity = contact.ToEntity();

        context.Contacts.Attach(entity);

        context.Entry(entity).State = EntityState.Deleted;
    }
}