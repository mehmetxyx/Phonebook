using AutoFixture;
using ContactManagement.Api.Controllers;
using ContactManagement.Application.Dtos;
using ContactManagement.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shared.Api.Common;
using Shared.Common;
namespace ContactManagement.Api.Tests;

public class ContactsControllerTests
{
    private readonly ILogger<ContactsController> logger;
    private readonly IContactService contactService;
    private readonly Fixture fixture;
    public ContactsControllerTests()
    {
        logger = Substitute.For<ILogger<ContactsController>>();
        contactService = Substitute.For<IContactService>();
        fixture = new Fixture();
    }
    [Fact]
    public async Task CreateContactAsync_WhenSuccesful_Returns_Created()
    {
        var contactRequest = fixture.Create<ContactCreateRequest>();
        var contactResponse = fixture.Create<ContactCreateResponse>();

        var response = Result<ContactCreateResponse>.Success(contactResponse);

        contactService.CreateContactAsync(contactRequest)
            .Returns(response);

        var contactsController = new ContactsController(contactService);

        ActionResult<ApiResponse<ContactCreateResponse>> result = await contactsController.CreateContactAsync(contactRequest);

        Assert.IsType<CreatedAtActionResult>(result.Result);
    }

    [Fact]
    public async Task CreateContactAsync_WhenFailed_Returns_BadRequest()
    {
        var contactRequest = fixture.Create<ContactCreateRequest>();

        var response = Result<ContactCreateResponse>.Failure("Can not create contact!");

        contactService.CreateContactAsync(contactRequest)
            .Returns(response);

        var contactsController = new ContactsController(contactService);

        ActionResult<ApiResponse<ContactCreateResponse>> result = await contactsController.CreateContactAsync(contactRequest);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetContactsAsync_WhenSuccessful_Returns_AllContacts()
    {
        var contactResponse = fixture.CreateMany<GetContactResponse>(3).ToList();

        var contacts = Result<List<GetContactResponse>>.Success(contactResponse);

        contactService.GetContactsAsync()
            .Returns(contacts);

        var contactsController = new ContactsController(contactService);

        ActionResult<ApiResponse<List<GetContactResponse>>> result = await contactsController.GetContactsAsync();   

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetContactsAsync_WhenFailed_Returns_Returns_BadRequest()
    {
        var contacts = Result<List<GetContactResponse>>.Failure("no cntacts found");

        contactService.GetContactsAsync()
            .Returns(contacts);

        var contactsController = new ContactsController(contactService);

        ActionResult<ApiResponse<List<GetContactResponse>>> result = await contactsController.GetContactsAsync();

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetContactAsync_WhenSuccessful_Returns_Contact()
    {
        var contact = fixture.Create<GetContactResponse>();
        var contactResponse = Result<GetContactResponse>.Success(contact);

        contactService.GetContactByIdAsync(contact.Id)
            .Returns(contactResponse);

        var contactsController = new ContactsController(contactService);

        ActionResult<ApiResponse<GetContactResponse>> result = await contactsController.GetContactAsync(contact.Id);

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetContactAsync_WhenFailed_Returns_Notfound()
    {
        var contact = fixture.Create<GetContactResponse>();
        var contactResponse = Result<GetContactResponse>.Failure("Contact not found!");

        contactService.GetContactByIdAsync(contact.Id)
            .Returns(contactResponse);

        var contactsController = new ContactsController(contactService);

        ActionResult<ApiResponse<GetContactResponse>> result = await contactsController.GetContactAsync(contact.Id);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task DeleteContactAsync_WhenSuccessful_Returns_OK() 
    { 
        var contactId = Guid.NewGuid();
        var response = Result<bool>.Success(true);
        contactService.DeleteContactAsync(contactId)
            .Returns(response);

        var contactsController = new ContactsController(contactService);
        ActionResult<ApiResponse<bool>> result = await contactsController.DeleteContactAsync(contactId);
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task DeleteContactAsync_WhenFailed_Returns_NotFound()
    {
        var contactId = Guid.NewGuid();
        var response = Result<bool>.Failure(false);
        contactService.DeleteContactAsync(contactId)
            .Returns(response);

        var contactsController = new ContactsController(contactService);
        ActionResult<ApiResponse<bool>> result = await contactsController.DeleteContactAsync(contactId);
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }
}
