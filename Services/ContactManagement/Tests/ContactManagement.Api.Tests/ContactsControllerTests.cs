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
    public ContactsControllerTests()
    {
        logger = Substitute.For<ILogger<ContactsController>>();
        contactService = Substitute.For<IContactService>();
    }
    [Fact]
    public async Task CreateContactAsync_WhenSuccesful_Returns_Created()
    {
        var request = new ContactCreateRequest();

        var response = Result<ContactCreateResponse>.Success(new ContactCreateResponse
        {
            Id = Guid.NewGuid()
        });

        contactService.CreateContactAsync(request)
            .Returns(response);

        var contactsController = new ContactsController(logger, contactService);

        ActionResult<ApiResponse<ContactCreateResponse>> result = await contactsController.CreateContactAsync(request);

        Assert.IsType<CreatedAtActionResult>(result.Result);
    }

    [Fact]
    public async Task CreateContactAsync_WhenFailed_Returns_BadRequest()
    {
        var request = new ContactCreateRequest();

        var response = Result<ContactCreateResponse>.Failure("Can not create contact!");

        contactService.CreateContactAsync(request)
            .Returns(response);

        var contactsController = new ContactsController(logger, contactService);

        ActionResult<ApiResponse<ContactCreateResponse>> result = await contactsController.CreateContactAsync(request);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetContactsAsync_WhenSuccessful_Returns_AllContacts()
    {

        var contacts = Result<List<GetContactResponse>>.Success(new List<GetContactResponse>
        {
            new GetContactResponse
            {
                Id = Guid.NewGuid()
            }
        });

        contactService.GetContactsAsync()
            .Returns(contacts);

        var contactsController = new ContactsController(logger, contactService);

        ActionResult<ApiResponse<List<GetContactResponse>>> result = await contactsController.GetContactsAsync();   

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetContactsAsync_WhenFailed_Returns_Returns_BadRequest()
    {
        var contacts = Result<List<GetContactResponse>>.Failure("no cntacts found");

        contactService.GetContactsAsync()
            .Returns(contacts);

        var contactsController = new ContactsController(logger, contactService);

        ActionResult<ApiResponse<List<GetContactResponse>>> result = await contactsController.GetContactsAsync();

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetContactAsync_WhenSuccessful_Returns_Contact()
    {
        var contactId = Guid.NewGuid();
        var contact = Result<GetContactResponse>.Success(
            new GetContactResponse
            {
                Id = contactId
            }
        );

        contactService.GetContactByIdAsync(contactId)
            .Returns(contact);

        var contactsController = new ContactsController(logger, contactService);

        ActionResult<ApiResponse<GetContactResponse>> result = await contactsController.GetContactAsync(contactId);

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetContactAsync_WhenFailed_Returns_Notfound()
    {
        var contactId = Guid.NewGuid();
        var contact = Result<GetContactResponse>.Failure("Contact not found!");

        contactService.GetContactByIdAsync(contactId)
            .Returns(contact);

        var contactsController = new ContactsController(logger, contactService);

        ActionResult<ApiResponse<GetContactResponse>> result = await contactsController.GetContactAsync(contactId);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task DeleteContactAsync_WhenSuccessful_Returns_OK() { 
        
        var contactId = Guid.NewGuid();
        var response = Result<bool>.Success(true);
        contactService.DeleteContactAsync(contactId)
            .Returns(response);

        var contactsController = new ContactsController(logger, contactService);
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

        var contactsController = new ContactsController(logger, contactService);
        ActionResult<ApiResponse<bool>> result = await contactsController.DeleteContactAsync(contactId);
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }
}
