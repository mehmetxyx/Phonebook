using ContactManagement.Domain.Entities;
using ContactManagement.Domain.Repositories;
using ContactManagement.Infrastructure.Data.Entities;
using ContactManagement.Infrastructure.Data.Mappers;
using Microsoft.EntityFrameworkCore;

namespace ContactManagement.Infrastructure.Data;

public class ContactDetailRepository: IContactDetailRepository
{
    private ContactManagementDbContext context;

    public ContactDetailRepository(ContactManagementDbContext context)
    {
        this.context = context;
    }

    public async Task AddAsync(ContactDetail contactDetail)
    {
        var entity = contactDetail.ToEntity();
        await context.ContactDetails.AddAsync(entity);
    }

    public async Task<List<ContactDetail>> GetAllAsync(Guid contactId)
    {
       var contactDetails = await context.ContactDetails
            .Where(cd => cd.ContactId == contactId)
            .ToListAsync();

        return contactDetails.Select(cd => cd.ToDomain())
            .ToList();
    }

    public async Task<ContactDetail?> GetByIdAsync(Guid contactId, Guid contactDetailId)
    {
        var contactDetail = await context.ContactDetails
            .Where(cd => cd.ContactId == contactId && cd.Id == contactDetailId)
            .FirstOrDefaultAsync();

        if (contactDetail is null)
            return null;
        
        return contactDetail.ToDomain();
    }

    public void Delete(ContactDetail contactDetail)
    {
         var trackedEntity = context.ChangeTracker.Entries<ContactDetailEntity>()
            .FirstOrDefault(e => e.Entity.Id == contactDetail.Id);

        if (trackedEntity is not null)
        {
            trackedEntity.State = EntityState.Deleted;
            return;
        }

        var entity = contactDetail.ToEntity();
        context.ContactDetails.Attach(entity);
        context.Entry(entity).State = EntityState.Deleted;
    }
}