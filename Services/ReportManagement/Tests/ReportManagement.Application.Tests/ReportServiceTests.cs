using AutoFixture;
using Microsoft.Extensions.Logging;
using NSubstitute;
using ReportManagement.Application.Dtos;
using ReportManagement.Application.Interfaces;
using ReportManagement.Application.Services;
using ReportManagement.Domain.Entities;
using ReportManagement.Domain.Repositories;
using Shared.Application.Messaging;
using Shared.Common;

namespace ReportManagement.Application.Tests;

public class ReportServiceTests
{
    private readonly IReportRepository reportRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly ReportService reportService;
    private readonly Fixture fixture;
    private readonly ILogger<ReportService> logger;
    private readonly IEventPublisher eventPublisher;

    public ReportServiceTests()
    {
        unitOfWork = Substitute.For<IUnitOfWork>();
        reportRepository = Substitute.For<IReportRepository>();
        logger = Substitute.For<ILogger<ReportService>>();
        eventPublisher = Substitute.For<IEventPublisher>();
        reportService = new ReportService(logger, unitOfWork, reportRepository, eventPublisher);
        fixture = new Fixture();
    }
    [Fact]
    public async Task CreateReportAsync_WhenSuccessful_Returns_True()
    {
        var response = Result<ReportResponse>.Success(fixture.Create<ReportResponse>());

        reportRepository.AddAsync(Arg.Any<Report>())
            .Returns(Task.FromResult(response));

        var result = await reportService.CreateReportAsync();

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task CreateReportAsync_WhenFailed_Returns_False()
    {
        reportRepository.When(x => x.AddAsync(Arg.Any<Report>()))
            .Do(x => { throw new Exception("Failed to create report"); });

        var result = await reportService.CreateReportAsync();

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task GetAllReportsAsync_WhenCalled_Returns_Reports()
    {
        var reports = fixture.CreateMany<Report>(5).ToList();

        reportRepository.GetAllAsync()
            .Returns(Task.FromResult(reports));

        var result = await reportService.GetAllReportsAsync();

        Assert.True(result.IsSuccess);
        Assert.Equal(5, result?.Value?.Count);
    }

    [Fact]
    public async Task GetAllReportsAsync_WhenNoReports_Returns_EmptyList()
    {
        reportRepository.GetAllAsync()
            .Returns(Task.FromResult(new List<Report>()));

        var result = await reportService.GetAllReportsAsync();
        
        Assert.NotNull(result);
        Assert.True(result?.IsSuccess);
    }

    [Fact]
    public async Task GetAllReportsAsync_WhenExceptionOccurs_Returns_Failure()
    {
        reportRepository.GetAllAsync()
            .Returns<Task<List<Report?>>>(x => throw new Exception());

        var result = await reportService.GetAllReportsAsync();

        Assert.False(result?.IsSuccess);
    }

    [Fact]
    public async Task GetReportByIdAsync_WhenCalled_Returns_Report()
    {
        var report = fixture.Create<Report>();

        reportRepository.GetByIdAsync(report.Id)
            .Returns(Task.FromResult<Report?>(report));

        var result = await reportService.GetReportByIdAsync(report.Id);

        Assert.True(result.IsSuccess);
        Assert.Equal(report.Id, result?.Value?.Id);
    }

    [Fact]
    public async Task GetReportByIdAsync_WhenReportNotFound_Returns_Null()
    {
        var reportId = Guid.NewGuid();
        reportRepository.GetByIdAsync(reportId)
            .Returns(Task.FromResult<Report?>(null));

        var result = await reportService.GetReportByIdAsync(reportId);

        Assert.False(result.IsSuccess);
        Assert.Null(result?.Value);
    }

    [Fact]
    public async Task GetReportByIdAsync_WhenExceptionOccurs_Returns_Failure()
    {
        var reportId = Guid.NewGuid();
        reportRepository.GetByIdAsync(reportId)
            .Returns<Task<Report?>>(x => throw new Exception());

        var result = await reportService.GetReportByIdAsync(reportId);

        Assert.False(result.IsSuccess);
    }
}
