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
        var reportDataResponse = fixture.CreateMany<ReportDataResponse>(3).ToList();
        var serviceResponse = Result<List<ReportDataResponse>>.Success(reportDataResponse);

        reportDataService.GetAllReportData()
            .Returns(serviceResponse);

        ActionResult<ApiResponse<List<ReportDataResponse>>> result = await reportDataController.GetAllReportDataAsync();

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetAllReportDataAsync_WhenFailed_Returns_NotFound()
    {
        var serviceResponse = Result<List<ReportDataResponse>>.Failure("No report data found!");
        
        reportDataService.GetAllReportData()
            .Returns(serviceResponse);

        ActionResult<ApiResponse<List<ReportDataResponse>>> result = await reportDataController.GetAllReportDataAsync();

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }
}
