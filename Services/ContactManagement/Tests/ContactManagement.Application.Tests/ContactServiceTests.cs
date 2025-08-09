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
    private readonly ContactService contactService;
    public ContactServiceTests()
    {
        logger = Substitute.For<ILogger<ContactService>>();
        contactRepository = Substitute.For<IContactRepository>();
        unitOfWork = Substitute.For<IUnitOfWork>();
        contactService = new ContactService(logger, unitOfWork, contactRepository);
        fixture = new Fixture();
    }

    [Fact]
    public async Task CreateContactAsync_WhenSuccessful_Returns_Success()
    {
        contactRepository.AddAsync(Arg.Any<Contact>())
            .Returns(Task.CompletedTask);

        var request = fixture.Create<ContactRequest>();

        var result = await contactService.CreateContactAsync(request);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task CreateContactAsync_WhenFailed_Returns_False()
    {
        contactRepository.When(x => x.AddAsync(Arg.Any<Contact>()))
            .Do(x => { throw new Exception("Failed to add contact"); });

        var request = fixture.Create<ContactRequest>();

        var result = await contactService.CreateContactAsync(request);

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task GetContactsAsync_WhenSuccessful_Returns_AllContacts()
    {
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
        contactRepository.When(x => x.GetAllAsync())
            .Do(x => { throw new Exception("Failed to retrieve contacts"); });

        var result = await contactService.GetContactsAsync();

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task GetContactByIdAsync_WhenContactFound_Returns_True()
    {
        var contact = fixture.Build<Contact>()
            .Create();

        contactRepository.GetByIdAsync(contact.Id)
            .Returns(Task.FromResult<Contact?>(contact));

        var result = await contactService.GetContactByIdAsync(contact.Id); ;

        Assert.True(result.IsSuccess);
        Assert.Equal(contact.Id, result?.Value?.Id);
    }

    [Fact]
    public async Task GetContactByIdAsync_WhenContactNotFound_Returns_False()
    {
        var contact = fixture.Create<Contact>();

        contactRepository.GetByIdAsync(contact.Id)
            .Returns(Task.FromResult<Contact?>(null));

        var result = await contactService.GetContactByIdAsync(contact.Id);

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task GetContactByIdAsync_WhenErrorOccurs_Returns_False()
    {
        var contact = fixture.Create<Contact>();

        contactRepository.GetByIdAsync(contact.Id)
            .Returns<Contact?>(x => throw new Exception());

        var result = await contactService.GetContactByIdAsync(contact.Id);

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task DeleteContactAsync_WhenContactDeleted_Returns_True()
    {
        var contact = fixture.Create<Contact>();

        contactRepository.GetByIdAsync(contact.Id)
            .Returns(Task.FromResult<Contact?>(contact));

        var result = await contactService.DeleteContactAsync(contact.Id);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task DeleteContactAsync_WhenContactNotFound_Returns_False()
    {
        var contact = fixture.Create<Contact>();

        contactRepository.GetByIdAsync(contact.Id)
            .Returns(Task.FromResult<Contact?>(null));

        var result = await contactService.DeleteContactAsync(contact.Id);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task DeleteContactAsync_WhenErrorOccurs_Returns_False()
    {
        var contact = fixture.Create<Contact>();

        contactRepository.GetByIdAsync(contact.Id)
            .Returns<Contact?>(x => throw new Exception());

        var result = await contactService.DeleteContactAsync(contact.Id);
        Assert.False(result.IsSuccess);
    }
}
