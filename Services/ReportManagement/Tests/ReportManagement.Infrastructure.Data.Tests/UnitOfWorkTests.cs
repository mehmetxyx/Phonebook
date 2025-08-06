using AutoFixture;
using ReportManagement.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ReportManagement.Infrastructure.Data.Tests;

public class UnitOfWorkTests
{
    private readonly ReportManagementDbContext context;
    private readonly Fixture fixture;
    private readonly UnitOfWork unitOfWork;

    public UnitOfWorkTests()
    {
        var options = new DbContextOptionsBuilder<ReportManagementDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        context = new ReportManagementDbContext(options);
        unitOfWork = new UnitOfWork(context);

        fixture = new Fixture();
    }

    [Fact]
    public async Task SaveAsync_WhenSuccessful_SavesReportEntity()
    {
        var entity = fixture.Build<ReportEntity>()
            .Without(c => c.ReportData)
            .Create();

        await context.Reports.AddAsync(entity);

        await unitOfWork.SaveAsync();

        var savedEntity = await context.Reports.FirstOrDefaultAsync(c => c.Id == entity.Id);

        Assert.NotNull(savedEntity);
        Assert.Equal(entity.Id, savedEntity.Id);
    }

    [Fact]
    public async Task SaveAsync_WhenOperationFails_Throws_ArgumentException()
    {
        var ReportId = Guid.NewGuid();
        var entity1 = fixture.Build<ReportEntity>()
            .Without(c => c.ReportData)
            .With(c => c.Id, ReportId)
            .Create();

        var entity2 = fixture.Build<ReportEntity>()
            .Without(c => c.ReportData)
            .With(c => c.Id, ReportId)
            .Create();

        await context.Reports.AddAsync(entity1);
        await unitOfWork.SaveAsync();

        // Detach the first entity to simulate a conflict
        context.Entry(entity1).State = EntityState.Detached;
        await context.Reports.AddAsync(entity2);

        await Assert.ThrowsAsync<ArgumentException>(unitOfWork.SaveAsync);    
    }
}
