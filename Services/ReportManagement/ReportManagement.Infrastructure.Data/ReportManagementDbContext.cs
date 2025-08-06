using Microsoft.EntityFrameworkCore;
using ReportManagement.Infrastructure.Data.Entities;

namespace ReportManagement.Infrastructure.Data;

public class ReportManagementDbContext: DbContext
{
	public ReportManagementDbContext(DbContextOptions<ReportManagementDbContext> options)
		: base(options)
	{

	}

    public DbSet<ReportEntity> Reports { get; set; }
}