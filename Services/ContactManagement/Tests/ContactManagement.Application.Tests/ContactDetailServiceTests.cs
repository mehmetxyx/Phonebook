using AutoFixture;
using ContactManagement.Application.Dtos;
using ContactManagement.Application.Interfaces;
using ContactManagement.Application.Services;
using ContactManagement.Domain.Entities;
using ContactManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace ContactManagement.Application.Tests;
public class ContactDetailServiceTests
{
    private readonly ILogger<ContactDetailService> logger;
    private readonly IContactDetailRepository contactDetailRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly Fixture fixture;
    private readonly ContactDetailService contactDetailService;
    private readonly Guid contactId;

    public ContactDetailServiceTests()
    {
        logger = Substitute.For<ILogger<ContactDetailService>>();
        contactDetailRepository = Substitute.For<IContactDetailRepository>();
        unitOfWork = Substitute.For<IUnitOfWork>();
        contactDetailService = new ContactDetailService(logger, unitOfWork, contactDetailRepository);
        fixture = new Fixture();
        contactId = Guid.NewGuid();
    }

    [Fact]
    public async Task CreateContactDetailsAsync_WhenSuccessful_Returns_Success()
    {
        contactDetailRepository.AddAsync(Arg.Any<ContactDetail>())
            .Returns(Task.CompletedTask);

        var request = fixture.Create<ContactDetailRequest>();

        var result = await contactDetailService.CreateContactDetailAsync(contactId, request);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task CreateContactDetailsAsync_WhenFailed_Returns_Failure()
    {
        contactDetailRepository.When(x => x.AddAsync(Arg.Any<ContactDetail>()))
            .Do(x => { throw new Exception("Failed to add contact detail"); });

        var request = fixture.Create<ContactDetailRequest>();

        var result = await contactDetailService.CreateContactDetailAsync(contactId, request);

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task GetAllContactDetailsAsync_WhenSuccessful_Returns_AllContactDetails()
    {
        var contactDetails = fixture.Build<ContactDetail>()
            .CreateMany(3)
            .ToList();

        contactDetailRepository.GetAllAsync(contactId)
            .Returns(contactDetails);

        var result = await contactDetailService.GetAllContactDetailsAsync(contactId);

        Assert.True(result.IsSuccess);
        Assert.Equal(3, result?.Value?.Count);
    }

    [Fact]
    public async Task GetAllContactDetailsAsync_WhenNoRecords_Returns_EmptyList()
    {
        contactDetailRepository.GetAllAsync(contactId)
            .Returns(new List<ContactDetail>());

        var result = await contactDetailService.GetAllContactDetailsAsync(contactId);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task GetAllContactDetailsAsync_WhenErrorOccurs_Returns_Failure()
    {
        contactDetailRepository.GetAllAsync(contactId)
            .Returns<List<ContactDetail>>(x => throw new Exception());

        var result = await contactDetailService.GetAllContactDetailsAsync(contactId);

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task GetContactDetailByIdAsync_WhenSuccessful_Returns_ContactDetail()
    {
        var contactDetailId = Guid.NewGuid();
        var contactDetail = fixture.Create<ContactDetail>();

        contactDetailRepository.GetByIdAsync(contactId, contactDetailId)
            .Returns(contactDetail);

        var result = await contactDetailService.GetContactDetailByIdAsync(contactId, contactDetailId);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task GetContactDetailByIdAsync_WhenNotFound_Returns_Null()
    {
        var contactDetailId = Guid.NewGuid();
        contactDetailRepository.GetByIdAsync(contactId, contactDetailId)
            .Returns(Task.FromResult<ContactDetail?>(null));

        var result = await contactDetailService.GetContactDetailByIdAsync(contactId, contactDetailId);

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task GetContactDetailByIdAsync_WhenErrorOccurs_Returns_Failure()
    {
        var contactDetailId = Guid.NewGuid();
        contactDetailRepository.GetByIdAsync(contactId, contactDetailId)
            .Returns<ContactDetail?>(x => throw new Exception());

        var result = await contactDetailService.GetContactDetailByIdAsync(contactId, contactDetailId);

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task DeleteContactDetailAsync_WhenSuccessful_Returns_Success()
    {
        var contactDetailId = Guid.NewGuid();
        var contactDetail = fixture.Build<ContactDetail>()
            .With(c => c.Id, contactDetailId)
            .With(c => c.ContactId, contactId)
            .Create();

        contactDetailRepository.GetByIdAsync(contactId, contactDetailId)
            .Returns(contactDetail);

        contactDetailRepository.Delete(contactDetail);

        var result = await contactDetailService.DeleteContactDetailAsync(contactId, contactDetailId);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task DeleteContactDetailAsync_WhenNotFound_Returns_Failure()
    {
        var contactDetailId = Guid.NewGuid();

        contactDetailRepository.GetByIdAsync(contactId, contactDetailId)
            .Returns(Task.FromResult<ContactDetail?>(null));

        var result = await contactDetailService.DeleteContactDetailAsync(contactId, contactDetailId);

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task DeleteContactDetailAsync_WhenErrorOccurs_Returns_Failure()
    {
        var contactDetailId = Guid.NewGuid();

        contactDetailRepository.GetByIdAsync(contactId, contactDetailId)
            .Returns<ContactDetail?>(x => throw new Exception());

        var result = await contactDetailService.DeleteContactDetailAsync(contactId, contactDetailId);

        Assert.False(result.IsSuccess);
    }
}
