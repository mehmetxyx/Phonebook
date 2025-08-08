using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ReportManagement.Api.Controllers;
using ReportManagement.Application.Dtos;
using ReportManagement.Application.Services;
using Shared.Api.Common;
using Shared.Common;

namespace ReportManagement.Api.Tests;
public class ReportDataControllerTests
{
    private readonly IReportDataService reportDataService;
    private readonly ReportDataController reportDataController;
    private readonly Fixture fixture;

    public ReportDataControllerTests()
    {
        reportDataService = Substitute.For<IReportDataService>();
        reportDataController = new ReportDataController(reportDataService);
        fixture = new Fixture();
    }

    [Fact]
    public async Task GetAllReportDataAsync_WhenSuccessful_Returns_AllReportData()
    {
        var reportId = Guid.NewGuid();
        var reportDataResponse = fixture.Build<ReportDataResponse>()
            .With(r => r.ReportId, reportId)
            .CreateMany(3)
            .ToList();

        var serviceResponse = Result<List<ReportDataResponse>>.Success(reportDataResponse);

        reportDataService.GetAllReportData(reportId)
            .Returns(serviceResponse);

        ActionResult<ApiResponse<List<ReportDataResponse>>> result = await reportDataController.GetAllReportDataAsync(reportId);

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetAllReportDataAsync_WhenFailed_Returns_NotFound()
    {
        Guid reportId = Guid.NewGuid();
        var serviceResponse = Result<List<ReportDataResponse>>.Failure("No report data found!");
        
        reportDataService.GetAllReportData(reportId)
            .Returns(serviceResponse);

        ActionResult<ApiResponse<List<ReportDataResponse>>> result = await reportDataController.GetAllReportDataAsync(reportId);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }
}
