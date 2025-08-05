using AutoFixture;
using ContactManagement.Domain.Entities;
using ContactManagement.Domain.Repositories;
using ContactManagement.Infrastructure.Data.Entities;
using ContactManagement.Infrastructure.Data.Mappers;
using Microsoft.EntityFrameworkCore;

namespace ContactManagement.Infrastructure.Data.Tests;
public class ContactDetailRepositoryTests
{
    private readonly ContactManagementDbContext context;
    private Fixture fixture;
    private ContactDetailRepository contactDetailRepository;
    private Guid contactId;

    public ContactDetailRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ContactManagementDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        context = new ContactManagementDbContext(options);
        fixture = new Fixture();
        contactDetailRepository = new ContactDetailRepository(context);
        contactId = Guid.NewGuid();
    }

    [Fact]
    public async Task AddAsync_WhenSuccessful_Returns_SavesContactDetailToDatabase()
    {
        var contactDetail = fixture.Create<ContactDetail>();

        await contactDetailRepository.AddAsync(contactDetail);
        await context.SaveChangesAsync();

        var savedContactDetail = await context.ContactDetails
            .FirstOrDefaultAsync(cd => cd.Id == contactDetail.Id);

        Assert.NotNull(savedContactDetail);
        Assert.Equal(contactDetail.Id, savedContactDetail.Id);
    }

    [Fact]
    public async Task GetAllAsync_WhenSuccessful_Returns_AllContactDetails()
    {
        var contactDetailEntities = fixture
            .Build<ContactDetailEntity>()
            .With(c => c.ContactId, contactId)
            .CreateMany(3)
            .ToList();
        
        await context.ContactDetails.AddRangeAsync(contactDetailEntities);
        await context.SaveChangesAsync();

        var savedContactDetails = await contactDetailRepository.GetAllAsync(contactId);

        Assert.NotNull(savedContactDetails);
        Assert.Equal(3, savedContactDetails.Count);
    }

    [Fact]
    public async Task GetAllAsync_WhenNoRecords_Returns_Empty()
    {
        var savedContactDetails = await contactDetailRepository.GetAllAsync(contactId);
        Assert.NotNull(savedContactDetails);
        Assert.Empty(savedContactDetails);
    }

    [Fact]
    public async Task GetByIdAsync_WhenSuccessful_Returns_ContactDetail()
    {
        var contactDetail = fixture.Build<ContactDetailEntity>()
            .With(c => c.ContactId, contactId)
            .Create();

        await context.ContactDetails.AddAsync(contactDetail);
        await context.SaveChangesAsync();

        var result = await contactDetailRepository.GetByIdAsync(contactId, contactDetail.Id);
        Assert.NotNull(result);
        Assert.Equal(contactDetail.Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotFound_Returns_Null()
    {
        var contactDetailId = Guid.NewGuid();
        var result = await contactDetailRepository.GetByIdAsync(contactId, contactDetailId);
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_WhenSuccessful_RemovesContactDetailFromDatabase()
    {
        var contactDetailEntity = fixture.Build<ContactDetailEntity>()
            .With(c => c.ContactId, contactId)
            .Create();

        await context.ContactDetails.AddAsync(contactDetailEntity);
        await context.SaveChangesAsync();

        contactDetailRepository.Delete(contactDetailEntity.ToDomain());
        await context.SaveChangesAsync();

        var deletedContactDetail = await context.ContactDetails
            .FirstOrDefaultAsync(cd => cd.Id == contactDetailEntity.Id);

        Assert.Null(deletedContactDetail);
    }
}
