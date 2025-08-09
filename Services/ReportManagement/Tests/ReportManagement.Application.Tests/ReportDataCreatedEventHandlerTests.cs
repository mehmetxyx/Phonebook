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
    private readonly IReportRepository reportRepository;
    private readonly IReportDataRepository reportDataRepository;
	private readonly ReportDataCreatedEventHandler reportDataCreatedEventHandler;
	private readonly Fixture fixture;
	
	public ReportDataCreatedEventHandlerTests()
	{
		this.logger = Substitute.For<ILogger<ReportDataCreatedEventHandler>>();
		this.unitOfWork = Substitute.For<IUnitOfWork>();
        this.reportRepository = Substitute.For<IReportRepository>();
        this.reportDataRepository = Substitute.For<IReportDataRepository>();
        this.reportDataCreatedEventHandler = new ReportDataCreatedEventHandler(logger, unitOfWork, reportRepository, reportDataRepository);
		this.fixture = new Fixture();
    }

	[Fact]
	public async Task SaveReportDataAsync_WhenSuccessful_Returns_True()
	{
		var reportDataCreatedEvent = fixture.Create<ReportDataCreatedEvent>();
		var reportData = fixture.CreateMany<ReportData>(3).ToList();
        var report = fixture.Create<Report>();

        reportRepository.GetByIdAsync(reportDataCreatedEvent.ReportId)
            .Returns(Task.FromResult<Report?>(report));

        reportDataRepository.SaveReportDataAsync(reportData)
			.Returns(Task.FromResult(Result<bool>.Success(true)));

		var result = await reportDataCreatedEventHandler.SaveReportDataAsync(reportDataCreatedEvent);
		Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task SaveReportDataAsync_WhenReportNotFound_Returns_False()
    {
        var reportDataCreatedEvent = fixture.Create<ReportDataCreatedEvent>();

        reportRepository.GetByIdAsync(reportDataCreatedEvent.ReportId)
            .Returns<Task<Report?>>(Task.FromResult<Report?>(null));

        var result = await reportDataCreatedEventHandler.SaveReportDataAsync(reportDataCreatedEvent);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task SaveReportDataAsync_WhenFailed_Returns_False()
    {
        var reportDataCreatedEvent = fixture.Create<ReportDataCreatedEvent>();
        var report = fixture.Create<Report>();

        reportRepository.GetByIdAsync(reportDataCreatedEvent.ReportId)
            .Returns(Task.FromResult<Report?>(report));

        reportDataRepository.SaveReportDataAsync(Arg.Any<List<ReportData>>())
			.Returns(x => throw new Exception("Database error"));

        var result = await reportDataCreatedEventHandler.SaveReportDataAsync(reportDataCreatedEvent);
        Assert.False(result.IsSuccess);
    }
}
