using AutoFixture;
using ContactManagement.Application.Dtos;
using ContactManagement.Application.Interfaces;
using ContactManagement.Application.Services;
using ContactManagement.Domain.Entities;
using ContactManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace ContactManagement.Application.Tests;

public class ContactServiceTests
{
    private readonly ILogger<ContactService> logger;
    private readonly IContactRepository contactRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly Fixture fixture;
    public ContactServiceTests()
    {
        logger = Substitute.For<ILogger<ContactService>>();
        contactRepository = Substitute.For<IContactRepository>();
        unitOfWork = Substitute.For<IUnitOfWork>();
        fixture = new Fixture();
    }
    [Fact]
    public async Task CreateContactAsync_WhenSuccessful_Returns_Success()
    {
        var contactService = new ContactService(logger, unitOfWork, contactRepository);
        contactRepository.AddAsync(Arg.Any<Contact>());

        var result = await contactService.CreateContactAsync(new ContactCreateRequest());

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task CreateContactAsync_WhenFailed_Returns_False()
    {
        var contactService = new ContactService(logger, unitOfWork, contactRepository);
        contactRepository.When(x => x.AddAsync(Arg.Any<Contact>()))
            .Do(x => { throw new Exception("Failed to add contact"); });

        var result = await contactService.CreateContactAsync(new ContactCreateRequest());

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task GetContactsAsync_WhenSuccessful_Returns_AllContacts()
    {
        var contactService = new ContactService(logger, unitOfWork, contactRepository);

        var contacts = fixture.Build<Contact>()
            .CreateMany(3)
            .ToList();

        contactRepository.GetAllAsync()
            .Returns(Task.FromResult(contacts));

        var result = await contactService.GetContactsAsync();

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task GetContactsAsync_WhenFailed_Returns_False()
    {
        var contactService = new ContactService(logger, unitOfWork, contactRepository);

        contactRepository.When(x => x.GetAllAsync())
            .Do(x => { throw new Exception("Failed to retrieve contacts"); });

        var result = await contactService.GetContactsAsync();

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task GetContactByIdAsync_WhenContactFound_Returns_True()
    {
        var contactService = new ContactService(logger, unitOfWork, contactRepository);
        var contactId = Guid.NewGuid();

        var contact = fixture.Build<Contact>()
            .With(x => x.Id, contactId)
            .Create();

        contactRepository.GetByIdAsync(contactId)
            .Returns(Task.FromResult(contact));

        var result = await contactService.GetContactByIdAsync(contactId); ;

        Assert.True(result.IsSuccess);
        Assert.Equal(contactId, result.Value.Id);
    }

    [Fact]
    public async Task GetContactByIdAsync_WhenContactNotFound_Returns_False()
    {
        var contactService = new ContactService(logger, unitOfWork, contactRepository);
        var contactId = Guid.NewGuid();

        var contact = fixture.Build<Contact>()
            .With(x => x.Id, contactId)
            .Create();

        contactRepository.GetByIdAsync(contactId)
            .Returns(Task.FromResult<Contact?>(null));

        var result = await contactService.GetContactByIdAsync(contactId); ;

        Assert.False(result.IsSuccess);;
    }

    [Fact]
    public async Task DeleteContactAsync_WhenContactDeleted_Returns_True()
    {
        var contactService = new ContactService(logger, unitOfWork, contactRepository);
        var contactId = Guid.NewGuid();

        contactRepository.GetByIdAsync(contactId)
            .Returns(Task.FromResult(new Contact { Id = contactId }));

        var result = await contactService.DeleteContactAsync(contactId);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task DeleteContactAsync_WhenContactNotFound_Returns_False()
    {
        var contactService = new ContactService(logger, unitOfWork, contactRepository);
        var contactId = Guid.NewGuid();
        
        contactRepository.GetByIdAsync(contactId)
            .Returns(Task.FromResult<Contact?>(null));

        var result = await contactService.DeleteContactAsync(contactId);
        Assert.False(result.IsSuccess);
    }
}
