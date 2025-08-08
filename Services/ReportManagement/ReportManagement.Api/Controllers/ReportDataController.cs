using Microsoft.AspNetCore.Mvc;
using ReportManagement.Application.Dtos;
using ReportManagement.Application.Services;
using Shared.Api.Common;

namespace ReportManagement.Api.Controllers;
[Route("api/reports/{reportId}")]
[ApiController]
public class ReportDataController : ControllerBase
{
    private IReportDataService reportDataService;

    public ReportDataController(IReportDataService reportDataService)
    {
        this.reportDataService = reportDataService;
    }

    [HttpGet("data")]
    public async Task<ActionResult<ApiResponse<List<ReportDataResponse>>>> GetAllReportDataAsync(Guid reportId)
    {
        var result = await reportDataService.GetAllReportData(reportId);
        if (result.IsSuccess)
            return Ok(result.ToApiResponse());

        return NotFound(result.ToApiResponse());
    }
}
