using ContactManagement.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactManagement.Infrastructure.Data;

public class ContactManagementDbContext: DbContext
{
    public ContactManagementDbContext(DbContextOptions<ContactManagementDbContext> dbOptions)
        : base(dbOptions)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContactManagementDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<ContactEntity> Contacts { get; set; }
    public DbSet<ContactDetailEntity> ContactDetails { get; set; }
}