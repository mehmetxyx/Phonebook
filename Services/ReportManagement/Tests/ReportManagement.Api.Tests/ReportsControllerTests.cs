using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ReportManagement.Api.Controllers;
using ReportManagement.Application.Dtos;
using ReportManagement.Application.Services;
using Shared.Api.Common;
using Shared.Common;

namespace ReportManagement.Api.Tests;

public class ReportsControllerTests
{
    private readonly IReportService reportService;
    private readonly ReportsController reportController;
    private readonly Fixture fixture;

    public ReportsControllerTests()
    {
        reportService = Substitute.For<IReportService>();
        reportController = new ReportsController(reportService);
        fixture = new Fixture();
    }

    [Fact]
    public async Task CreateReportAsync_WhenSuccessful_Returns_OK()
    {
        var reportResponse = fixture.Create<ReportResponse>();

        var response = Result<ReportResponse>.Success(reportResponse);

        reportService.CreateReportAsync()
            .Returns(response);

        ActionResult<ApiResponse<ReportResponse>> result = await reportController.CreateReportAsync();

        Assert.IsType<CreatedAtActionResult>(result.Result);
    }

    [Fact]
    public async Task CreateReportAsync_WhenFailed_Returns_BadRequest()
    {
        var response = Result<ReportResponse>.Failure("Cannot create report!");

        reportService.CreateReportAsync()
            .Returns(response);

        ActionResult<ApiResponse<ReportResponse>> result = await reportController.CreateReportAsync();
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetAllReportsAsync_WhenSuccessful_Returns_AllReports()
    {
        var reportResponses = fixture.CreateMany<ReportResponse>(3).ToList();
        var response = Result<List<ReportResponse>>.Success(reportResponses);

        reportService.GetAllReportsAsync()
            .Returns(response);

        ActionResult<ApiResponse<List<ReportResponse>>> result = await reportController.GetAllReportsAsync();
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetAllReportsAsync_WhenNoReports_Returns_EmptyList()
    {
        var response = Result<List<ReportResponse>>.Success(new List<ReportResponse>());

        reportService.GetAllReportsAsync()
            .Returns(response);

        ActionResult<ApiResponse<List<ReportResponse>>> result = await reportController.GetAllReportsAsync();
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetAllReportsAsync_WhenAnErrrorOccurs_Returns_NotFound()
    {
        var response = Result<List<ReportResponse>>.Failure(new List<ReportResponse>());

        reportService.GetAllReportsAsync()
            .Returns(response);

        ActionResult<ApiResponse<List<ReportResponse>>> result = await reportController.GetAllReportsAsync();
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetReportByIdAsync_WhenSuccessful_Returns_Report()
    {
        var reportId = Guid.NewGuid();
        var reportResponse = fixture.Create<ReportResponse>();

        var response = Result<ReportResponse>.Success(reportResponse);
        reportService.GetReportByIdAsync(reportId)
            .Returns(response);

        ActionResult<ApiResponse<ReportResponse>> result = await reportController.GetReportByIdAsync(reportId);
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetReportByIdAsync_WhenNotFound_Returns_NotFound()
    {
        var reportId = Guid.NewGuid();
        var response = Result<ReportResponse>.Failure("Report not found");
        reportService.GetReportByIdAsync(reportId)
            .Returns(response);

        ActionResult<ApiResponse<ReportResponse>> result = await reportController.GetReportByIdAsync(reportId);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }
}
