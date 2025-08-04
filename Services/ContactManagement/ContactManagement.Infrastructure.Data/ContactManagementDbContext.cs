using ContactManagement.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactManagement.Infrastructure.Data;

public class ContactManagementDbContext: DbContext
{
    public ContactManagementDbContext(DbContextOptions<ContactManagementDbContext> dbOptions)
        : base(dbOptions)
    {
    }

    public DbSet<ContactEntity> Contacts { get; set; }
}