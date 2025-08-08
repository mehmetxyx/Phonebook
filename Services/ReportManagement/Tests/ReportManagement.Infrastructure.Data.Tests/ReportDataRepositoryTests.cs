using AutoFixture;
using Microsoft.EntityFrameworkCore;
using ReportManagement.Infrastructure.Data.Entities;
using ReportManagement.Infrastructure.Data.Repositories;
using ReportManagement.Infrastructure.Data.Mappers;

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
        var reportId = Guid.NewGuid();
        var reportDataEntities = fixture.Build<ReportDataEntity>()
            .With(r => r.ReportId, reportId)
            .Without(r => r.Report)
            .CreateMany(3)
            .ToList();

        await context.ReportData.AddRangeAsync(reportDataEntities);
        await context.SaveChangesAsync();

        var savedReports = await reportDataRepository.GetAllAsync(reportId);

        Assert.NotNull(savedReports);
        Assert.Equal(3, savedReports.Count);
    }

    [Fact]
    public async Task SaveReportDataAsync_WhenSuccessful_SavesReportData()
    {
        var reportDataEntities = fixture.Build<ReportDataEntity>()
            .Without(r => r.Report)
            .CreateMany(3)
            .ToList();

        var reportData = reportDataEntities.Select(r => r.ToDomain()).ToList();

        await reportDataRepository.SaveReportDataAsync(reportData);
        context.SaveChanges();

        var savedReports = await context.ReportData.ToListAsync();
        Assert.NotNull(savedReports);
        Assert.Equal(3, savedReports.Count);
    }
}
