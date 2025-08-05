using AutoFixture;
using ContactManagement.Api.Controllers;
using ContactManagement.Application.Dtos;
using ContactManagement.Application.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shared.Api.Common;
using Shared.Common;
namespace ContactManagement.Api.Tests;

public class ContactsControllerTests
{
    private readonly IContactService contactService;
    private readonly Fixture fixture;
    private readonly ContactsController contactsController;
    public ContactsControllerTests()
    {
        contactService = Substitute.For<IContactService>();
        contactsController = new ContactsController(contactService);
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

        ActionResult<ApiResponse<ContactCreateResponse>> result = await contactsController.CreateContactAsync(contactRequest);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetContactsAsync_WhenSuccessful_Returns_AllContacts()
    {
        var contactResponse = fixture.CreateMany<ContactGetResponse>(3).ToList();

        var contacts = Result<List<ContactGetResponse>>.Success(contactResponse);

        contactService.GetContactsAsync()
            .Returns(contacts);

        ActionResult<ApiResponse<List<ContactGetResponse>>> result = await contactsController.GetContactsAsync();   

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetContactsAsync_WhenFailed_Returns_Returns_BadRequest()
    {
        var contacts = Result<List<ContactGetResponse>>.Failure("no cntacts found");

        contactService.GetContactsAsync()
            .Returns(contacts);

        ActionResult<ApiResponse<List<ContactGetResponse>>> result = await contactsController.GetContactsAsync();

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetContactByIdAsync_WhenSuccessful_Returns_Contact()
    {
        var contact = fixture.Create<ContactGetResponse>();
        var contactResponse = Result<ContactGetResponse>.Success(contact);

        contactService.GetContactByIdAsync(contact.Id)
            .Returns(contactResponse);

        ActionResult<ApiResponse<ContactGetResponse>> result = await contactsController.GetContactByIdAsync(contact.Id);

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetContactByIdAsync_WhenFailed_Returns_Notfound()
    {
        var contact = fixture.Create<ContactGetResponse>();
        var contactResponse = Result<ContactGetResponse>.Failure("Contact not found!");

        contactService.GetContactByIdAsync(contact.Id)
            .Returns(contactResponse);

        ActionResult<ApiResponse<ContactGetResponse>> result = await contactsController.GetContactByIdAsync(contact.Id);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task DeleteContactAsync_WhenSuccessful_Returns_OK() 
    { 
        var contactId = Guid.NewGuid();
        var response = Result<bool>.Success(true);
        contactService.DeleteContactAsync(contactId)
            .Returns(response);

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

        ActionResult<ApiResponse<bool>> result = await contactsController.DeleteContactAsync(contactId);
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }
}
