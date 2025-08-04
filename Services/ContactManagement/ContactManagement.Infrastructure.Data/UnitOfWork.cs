using ContactManagement.Application.Interfaces;

namespace ContactManagement.Infrastructure.Data;

public class UnitOfWork: IUnitOfWork
{
    private ContactManagementDbContext context;

    public UnitOfWork(ContactManagementDbContext context)
    {
        this.context = context;
    }

    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }
}