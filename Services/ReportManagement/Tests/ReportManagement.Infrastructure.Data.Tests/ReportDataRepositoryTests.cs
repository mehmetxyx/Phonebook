using AutoFixture;
using Microsoft.EntityFrameworkCore;
using ReportManagement.Infrastructure.Data.Entities;
using ReportManagement.Infrastructure.Data.Repositories;

namespace ReportManagement.Infrastructure.Data.Tests;

public class ReportDataRepositoryTests
{
    private readonly ReportManagementDbContext context;
    private readonly Fixture fixture;
    private readonly ReportDataRepository reportDataRepository;

    public ReportDataRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ReportManagementDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        context = new ReportManagementDbContext(options);
        reportDataRepository = new ReportDataRepository(context);
        fixture = new Fixture();
    }

    [Fact]
    public async Task GetAllAsync_WhenSuccessful_Returns_AllReports()
    {
        var reportDataEntities = fixture.Build<ReportDataEntity>()
            .CreateMany(3)
            .ToList();

        await context.ReportData.AddRangeAsync(reportDataEntities);
        await context.SaveChangesAsync();

        var savedReports = await reportDataRepository.GetAllAsync();

        Assert.NotNull(savedReports);
        Assert.Equal(3, savedReports.Count);
    }
}
