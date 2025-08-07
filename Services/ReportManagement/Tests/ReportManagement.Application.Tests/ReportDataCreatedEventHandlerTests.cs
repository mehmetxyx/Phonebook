using AutoFixture;
using Microsoft.Extensions.Logging;
using NSubstitute;
using ReportManagement.Application.EventHandlers;
using ReportManagement.Application.Interfaces;
using ReportManagement.Domain.Entities;
using ReportManagement.Domain.Repositories;
using Shared.Common;
using Shared.Contracts;

namespace ReportManagement.Application.Tests;
public class ReportDataCreatedEventHandlerTests
{
	private ILogger<ReportDataCreatedEventHandler> logger;
	private readonly IUnitOfWork unitOfWork;
	private readonly IReportDataRepository reportDataRepository;
	private readonly ReportDataCreatedEventHandler reportDataCreatedEventHandler;
	private readonly Fixture fixture;
	
	public ReportDataCreatedEventHandlerTests()
	{
		this.logger = Substitute.For<ILogger<ReportDataCreatedEventHandler>>();
		this.unitOfWork = Substitute.For<IUnitOfWork>();
        this.reportDataRepository = Substitute.For<IReportDataRepository>();
        this.reportDataCreatedEventHandler = new ReportDataCreatedEventHandler(logger, unitOfWork, reportDataRepository);
		this.fixture = new Fixture();
    }

	[Fact]
	public async Task SaveReportDataAsync_WhenSuccessful_Returns_True()
	{
		var reportDataCreatedEvent = fixture.Create<ReportDataCreatedEvent>();
		var reportData = fixture.CreateMany<ReportData>(3).ToList();

        reportDataRepository.SaveReportDataAsync(reportData)
			.Returns(Task.FromResult(Result<bool>.Success(true)));

		var result = await reportDataCreatedEventHandler.SaveReportDataAsync(reportDataCreatedEvent);
		Assert.True(result.IsSuccess);
    }


    [Fact]
    public async Task SaveReportDataAsync_WhenFailed_Returns_False()
    {
        var reportDataCreatedEvent = fixture.Create<ReportDataCreatedEvent>();

		reportDataRepository.When(x => x.SaveReportDataAsync(Arg.Any<List<ReportData>>()))
			.Do(x => throw new Exception("Database error"));

        var result = await reportDataCreatedEventHandler.SaveReportDataAsync(reportDataCreatedEvent);
        Assert.False(result.IsSuccess);
    }
}
