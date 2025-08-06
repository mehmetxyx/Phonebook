using Microsoft.EntityFrameworkCore;
using ReportManagement.Infrastructure.Data.Entities;

namespace ReportManagement.Infrastructure.Data;

public class ReportManagementDbContext: DbContext
{
	public ReportManagementDbContext(DbContextOptions<ReportManagementDbContext> options)
		: base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReportManagementDbContext).Assembly);
    }

    public DbSet<ReportEntity> Reports { get; set; }
    public DbSet<ReportDataEntity> ReportData { get; set; }
}