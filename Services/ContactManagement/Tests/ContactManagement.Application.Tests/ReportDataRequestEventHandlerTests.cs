using AutoFixture;
using ContactManagement.Application.Dtos;
using ContactManagement.Application.EventHandlers;
using ContactManagement.Application.Repositories;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shared.Contracts;

namespace ContactManagement.Application.Tests;
public class ReportDataRequestEventHandlerTests
{
    private readonly ILogger<ReportDataRequestEventHandler> logger;
    public ReportDataRequestEventHandlerTests()
    {
        logger = Substitute.For<ILogger<ReportDataRequestEventHandler>>();
    }
    [Fact]
    public async Task GenerateReportDataAsync_WhenSuccessful_Returns_ReportData()
    {
        var contactReportDataRepository = Substitute.For<IContactReportDataRepository>();
        var fixture = new Fixture();
        var contactReportData = fixture.CreateMany<ContactReportData>(5).ToList();
        var requestEvent = fixture.Create<ReportDataRequestedEvent>();

        contactReportDataRepository.GetContactReportDataAsync()
            .Returns(Task.FromResult(contactReportData));

        var reportDataRequestEventHandler = new ReportDataRequestEventHandler(contactReportDataRepository, logger);
        
        var result = await reportDataRequestEventHandler.GenerateReportDataAsync(requestEvent);
     
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task GenerateReportDataAsync_WhenNoDataFound_Returns_Failure()
    {
        var contactReportDataRepository = Substitute.For<IContactReportDataRepository>();
        var fixture = new Fixture();
        var requestEvent = fixture.Create<ReportDataRequestedEvent>();
        
        contactReportDataRepository.GetContactReportDataAsync()
            .Returns<Task<List<ContactReportData>>>(_ => throw new Exception());

        var reportDataRequestEventHandler = new ReportDataRequestEventHandler(contactReportDataRepository, logger);
        
        var result = await reportDataRequestEventHandler.GenerateReportDataAsync(requestEvent);
     
        Assert.False(result.IsSuccess);
        Assert.Contains("Failed to generate report data", result.Message);
    }
}
