using AutoFixture;
using ContactManagement.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactManagement.Infrastructure.Data.Tests;

public class UnitOfWorkTests
{
    private readonly ContactManagementDbContext context;
    private readonly Fixture fixture;
    public UnitOfWorkTests()
    {
        var options = new DbContextOptionsBuilder<ContactManagementDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        context = new ContactManagementDbContext(options);
        fixture = new Fixture();
    }

    [Fact]
    public async Task SaveAsync_WhenSuccessful_SavesContactEntity()
    {
        var unitOfWork = new UnitOfWork(context);
        var entity = fixture.Build<ContactEntity>().Create();

        await context.Contacts.AddAsync(entity);

        await unitOfWork.SaveAsync();

        var savedEntity = context.Contacts.FirstOrDefaultAsync(c => c.Id == entity.Id);

        Assert.NotNull(savedEntity);
        Assert.Equal(entity.Id, savedEntity.Result.Id);
    }

    [Fact]
    public async Task SaveAsync_WhenOperationFails_Throws_Exception()
    {
        var unitOfWork = new UnitOfWork(context);

        var entity = fixture.Build<ContactEntity>().Create();
        entity.Name = null; // Invalid entity (Name is required)

        await context.Contacts.AddAsync(entity);

        await Assert.ThrowsAsync<DbUpdateException>(unitOfWork.SaveAsync);    
    }
}
