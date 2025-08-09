using AutoFixture;
using Microsoft.EntityFrameworkCore;
using ReportManagement.Domain.Entities;
using ReportManagement.Domain.Enums;
using ReportManagement.Infrastructure.Data.Entities;
using ReportManagement.Infrastructure.Data.Repositories;

namespace ReportManagement.Infrastructure.Data.Tests;

public class ReportRepositoryTests
{
    private readonly ReportManagementDbContext context;
    private readonly Fixture fixture;
    private readonly ReportRepository reportRepository;

    public ReportRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ReportManagementDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        context = new ReportManagementDbContext(options);
        reportRepository = new ReportRepository(context);
        fixture = new Fixture();
    }
    
    [Fact]
    public async Task AddReportAsync_WhenSuccessful_Returns_SavesReportToDatabase()
    {
        var report = fixture.Build<Report>().Create();
        
        await reportRepository.AddAsync(report);
        await context.SaveChangesAsync();

        var savedReport = await context.Reports.FirstOrDefaultAsync(r => r.Id == report.Id);

        Assert.NotNull(savedReport);
        Assert.Equal(report.Id, savedReport.Id);
    }

    [Fact]
    public async Task GetAllAsync_WhenSuccessful_Returns_AllReports()
    {
        var reportEntities = fixture.Build<ReportEntity>()
            .Without(r => r.ReportData)
            .CreateMany(3)
            .ToList();

        await context.Reports.AddRangeAsync(reportEntities);
        await context.SaveChangesAsync();

        var savedReports = await reportRepository.GetAllAsync();

        Assert.NotNull(savedReports);
        Assert.Equal(3, savedReports.Count);
    }

    [Fact]
    public async Task GetAllAsync_WhenNoRecords_Returns_Empty()
    {
        var savedReports = await reportRepository.GetAllAsync();
        Assert.NotNull(savedReports);
        Assert.Empty(savedReports);
    }

    [Fact]
    public async Task GetByIdAsync_WhenReportExists_Returns_Report()
    {
        var report = fixture.Build<ReportEntity>()
            .Without(r => r.ReportData)
            .Create();
        
        await context.Reports.AddAsync(report);
        await context.SaveChangesAsync();

        var retrievedReport = await reportRepository.GetByIdAsync(report.Id);
        
        Assert.NotNull(retrievedReport);
        Assert.Equal(report.Id, retrievedReport?.Id);
    }

    [Fact]
    public async Task GetByIdAsync_WhenReportDoesNotExist_Returns_Null()
    {
        var reportId = Guid.NewGuid();

        var retrievedReport = await reportRepository.GetByIdAsync(reportId);

        Assert.Null(retrievedReport);
    }

    [Fact]
    public async Task Update_WhenSuccessful_Returns_SavesReportToDatabase()
    {
        var report = fixture.Build<Report>()
            .With(r => r.Status, ReportStatus.Pending)
            .Create();
        
        await reportRepository.AddAsync(report);
        await context.SaveChangesAsync();

        report.Status = ReportStatus.Completed;

        reportRepository.Update(report);
        await context.SaveChangesAsync();

        var savedReport = await context.Reports.FirstOrDefaultAsync(r => r.Id == report.Id);

        Assert.NotNull(savedReport);
        Assert.Equal(report.Id, savedReport.Id);
        Assert.Equal(report.Status, savedReport.Status);
    }

}
