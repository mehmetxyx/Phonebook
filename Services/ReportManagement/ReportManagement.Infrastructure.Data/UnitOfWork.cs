using ReportManagement.Application.Interfaces;

namespace ReportManagement.Infrastructure.Data;

public class UnitOfWork: IUnitOfWork
{
    private ReportManagementDbContext context;

    public UnitOfWork(ReportManagementDbContext context)
    {
        this.context = context;
    }

    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }
}