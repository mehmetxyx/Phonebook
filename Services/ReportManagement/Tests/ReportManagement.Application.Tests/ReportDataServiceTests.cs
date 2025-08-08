using AutoFixture;
using Microsoft.Extensions.Logging;
using NSubstitute;
using ReportManagement.Application.Services;
using ReportManagement.Domain.Repositories;
using ReportManagement.Application.Interfaces;
using ReportManagement.Domain.Entities;

namespace ReportManagement.Application.Tests;

public class ReportDataServiceTests
{
    private readonly IReportDataRepository reportDataRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly ReportDataService reportDataService;
    private readonly Fixture fixture;
    private readonly ILogger<ReportDataService> logger;

    public ReportDataServiceTests()
    {
        unitOfWork = Substitute.For<IUnitOfWork>();
        reportDataRepository = Substitute.For<IReportDataRepository>();
        logger = Substitute.For<ILogger<ReportDataService>>();
        reportDataService = new ReportDataService(logger, unitOfWork, reportDataRepository);
        fixture = new Fixture();
    }

    [Fact]
    public async Task GetAllReportData_WhenSuccessful_ReturnsListOfReportDataResponse()
    {
        var reportId = Guid.NewGuid();
        var expectedReportData = fixture.Build<ReportData>()
            .With(r => r.ReportId, reportId)
            .CreateMany(3).ToList();

        reportDataRepository.GetAllAsync(reportId)
            .Returns(expectedReportData);

        var result = await reportDataService.GetAllReportData(reportId);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task GetAllReportData_WhenNoDataFound_ReturnsEmptyList()
    {
        var reportId = Guid.NewGuid();
        reportDataRepository.GetAllAsync(reportId)
            .Returns(new List<ReportData>());

        var result = await reportDataService.GetAllReportData(reportId);

        Assert.True(result.IsSuccess);
        Assert.Equal("No report data found.", result.Message);
    }
}
