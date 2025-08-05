using AutoFixture;
using ContactManagement.Domain.Entities;
using ContactManagement.Infrastructure.Data.Entities;
using ContactManagement.Infrastructure.Data.Mappers;
using Microsoft.EntityFrameworkCore;

namespace ContactManagement.Infrastructure.Data.Tests;

public class ContactRepositoryTests
{
    private readonly ContactManagementDbContext context;
    private Fixture fixture;
    private ContactRepository contactRepository;
    public ContactRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ContactManagementDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        context = new ContactManagementDbContext(options);
        fixture = new Fixture();
        contactRepository = new ContactRepository(context);
    }

    [Fact]
    public async Task AddContactAsync_WhenSuccessful_Returns_SavesContactToDatabase()
    {
        var contact = fixture.Build<Contact>().Create();

        await contactRepository.AddAsync(contact);
        await context.SaveChangesAsync();

        var savedContact = await context.Contacts.FirstOrDefaultAsync(c => c.Id == contact.Id);

        Assert.NotNull(savedContact);
        Assert.Equal(contact.Id, savedContact.Id);
    }

    [Fact]
    public async Task GetAllAsync_WhenSuccessful_Returns_AllContacts()
    {
        var contacts = fixture.CreateMany<ContactEntity>(3).ToList();

        await context.Contacts.AddRangeAsync(contacts);
        await context.SaveChangesAsync();

        var savedContacts = await contactRepository.GetAllAsync();

        Assert.NotNull(savedContacts);
        Assert.Equal(3, savedContacts.Count);
    }

    [Fact]
    public async Task GetAllAsync_WhenNoRecords_Returns_Empty()
    {
        var savedContacts = await contactRepository.GetAllAsync();

        Assert.NotNull(savedContacts);
        Assert.Empty(savedContacts);
    }

    [Fact]
    public async Task GetByIdAsync_WhenContactExists_Returns_Contact()
    {
        var contact = fixture.Create<ContactEntity>();

        await context.Contacts.AddAsync(contact);
        await context.SaveChangesAsync();

        var retrievedContact = await contactRepository.GetByIdAsync(contact.Id);

        Assert.NotNull(retrievedContact);
        Assert.Equal(contact.Id, retrievedContact?.Id);
    }

    [Fact]
    public async Task GetByIdAsync_WhenContactDoesNotExists_Returns_null()
    {
        var contactId = Guid.NewGuid();
        var retrievedContact = await contactRepository.GetByIdAsync(contactId);

        Assert.Null(retrievedContact);
    }

    [Fact] 
    public async Task DeleteAsync_WhenContactExists_DeletesContact()
    {
        var contact = fixture.Create<ContactEntity>();

        await context.Contacts.AddAsync(contact);
        await context.SaveChangesAsync();

        contactRepository.Delete(contact.ToDomain());
        await context.SaveChangesAsync();

        var deletedContact = await context.Contacts.FirstOrDefaultAsync(c => c.Id == contact.Id);
        Assert.Null(deletedContact);
    }

    [Fact]
    public async Task DeleteAsync_WhenContactIsDetached_DeletesSuccessfully()
    {
        var contact = fixture.Create<ContactEntity>();
        await context.Contacts.AddAsync(contact);
        await context.SaveChangesAsync();

        context.Entry(contact).State = EntityState.Detached;

        contactRepository.Delete(contact.ToDomain());
        await context.SaveChangesAsync();

        var deleted = await context.Contacts.FirstOrDefaultAsync(c => c.Id == contact.Id);
        Assert.Null(deleted);
    }
}
