using Microsoft.AspNetCore.Mvc;
using ReportManagement.Application.Dtos;
using ReportManagement.Application.Services;
using Shared.Api.Common;

namespace ReportManagement.Api.Controllers;
[Route("api/reports")]
[ApiController]
public class ReportsController : ControllerBase
{
    private IReportService reportService;

    public ReportsController(IReportService reportService)
    {
        this.reportService = reportService;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ReportResponse>>> CreateReportAsync(ReportRequest reportRequest)
    {
        var result = await reportService.CreateReportAsync(reportRequest);

        if (result.IsSuccess)
            return CreatedAtAction(nameof(GetReportByIdAsync), new { reportId = result.Value.Id }, result.ToApiResponse());

        return BadRequest(result.ToApiResponse());
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<ReportResponse>>>> GetAllReportsAsync()
    {
        var result = await reportService.GetAllReportsAsync();
        if (result.IsSuccess)
            return Ok(result.ToApiResponse());

        return NotFound(result.ToApiResponse());
    }

    [HttpGet("{reportId}")]
    public async Task<ActionResult<ApiResponse<ReportResponse>>> GetReportByIdAsync(Guid reportId)
    {
        var result = await reportService.GetReportByIdAsync(reportId);
        if (result.IsSuccess)
            return Ok(result.ToApiResponse());

        return NotFound(result.ToApiResponse());
    }
}
