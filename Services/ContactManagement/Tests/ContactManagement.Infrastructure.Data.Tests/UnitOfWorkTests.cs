using AutoFixture;
using ContactManagement.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactManagement.Infrastructure.Data.Tests;

public class UnitOfWorkTests
{
    private readonly ContactManagementDbContext context;
    private readonly Fixture fixture;
    private readonly UnitOfWork unitOfWork;
    public UnitOfWorkTests()
    {
        var options = new DbContextOptionsBuilder<ContactManagementDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        context = new ContactManagementDbContext(options);
        unitOfWork = new UnitOfWork(context);

        fixture = new Fixture();
    }

    [Fact]
    public async Task SaveAsync_WhenSuccessful_SavesContactEntity()
    {
        var entity = fixture.Create<ContactEntity>();

        await context.Contacts.AddAsync(entity);

        await unitOfWork.SaveAsync();

        var savedEntity = await context.Contacts.FirstOrDefaultAsync(c => c.Id == entity.Id);

        Assert.NotNull(savedEntity);
        Assert.Equal(entity.Id, savedEntity.Id);
    }

    [Fact]
    public async Task SaveAsync_WhenOperationFails_Throws_ArgumentException()
    {
        var contactId = Guid.NewGuid();
        var entity1 = fixture.Build<ContactEntity>().With(c => c.Id, contactId).Create();
        var entity2 = fixture.Build<ContactEntity>().With(c => c.Id, contactId).Create();

        await context.Contacts.AddAsync(entity1);
        await unitOfWork.SaveAsync();

        // Detach the first entity to simulate a conflict
        context.Entry(entity1).State = EntityState.Detached;
        await context.Contacts.AddAsync(entity2);

        await Assert.ThrowsAsync<ArgumentException>(unitOfWork.SaveAsync);    
    }
}
