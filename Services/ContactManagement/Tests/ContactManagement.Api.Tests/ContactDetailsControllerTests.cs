using AutoFixture;
using ContactManagement.Api.Controllers;
using ContactManagement.Application.Dtos;
using ContactManagement.Application.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shared.Api.Common;
using Shared.Common;

namespace ContactManagement.Api.Tests;
public class ContactDetailsControllerTests
{
    private readonly ContactDetailsController controller;
    private readonly IContactDetailService contactDetailService;
    private readonly Fixture fixture;
    private readonly Guid contactId;

    public ContactDetailsControllerTests()
    {
        contactDetailService = Substitute.For<IContactDetailService>();
        controller = new ContactDetailsController(contactDetailService);
        fixture = new Fixture();
        contactId = Guid.NewGuid();
    }

    [Fact]
    public async Task ContactDetailCreateAsync_WhenSuccessful_Returns_Created()
    {
        var createRequest = fixture.Create<ContactDetailCreateRequest>();
        var createResponse = fixture.Create<ContactDetailCreateResponse>();
        

        var response = Result<ContactDetailCreateResponse>.Success(createResponse);

        contactDetailService.CreateContactDetailAsync(contactId, createRequest)
            .Returns(response);

        ActionResult<ApiResponse<ContactDetailCreateResponse>> result = await controller.ContactDetailCreateAsync(contactId, createRequest);

        Assert.IsType<CreatedAtActionResult>(result.Result);
    }

    [Fact]
    public async Task ContactDetailCreateAsync_WhenFailed_Returns_BadRequest()
    {
        var createRequest = fixture.Create<ContactDetailCreateRequest>();
        var response = Result<ContactDetailCreateResponse>.Failure("Can not create contact detail!");

        contactDetailService.CreateContactDetailAsync(contactId, createRequest)
            .Returns(response);

        ActionResult<ApiResponse<ContactDetailCreateResponse>> result = await controller.ContactDetailCreateAsync(contactId, createRequest);
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetAllContactDetailsAsync_WhenSuccessful_Returns_AllContactDetails()
    {
        var contactDetails = fixture.CreateMany<ContactDetailGetResponse>(3).ToList();
        var response = Result<List<ContactDetailGetResponse>>.Success(contactDetails);

        contactDetailService.GetAllContactDetailsAsync(contactId)
            .Returns(response);

        ActionResult<ApiResponse<List<ContactDetailGetResponse>>> result = await controller.GetAllContactDetailsAsync(contactId);
        
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetAllContactDetailsAsync_WhenNoRecords_Returns_NotFound()
    {
        var response = Result<List<ContactDetailGetResponse>>.Failure("No contact details found");
        
        contactDetailService.GetAllContactDetailsAsync(contactId)
            .Returns(response);

        ActionResult<ApiResponse<List<ContactDetailGetResponse>>> result = await controller.GetAllContactDetailsAsync(contactId);
        
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetContactDetailByIdAsync_WhenSuccessful_Returns_ContactDetail()
    {
        var contactDetailId = Guid.NewGuid();
        var contactDetail = fixture.Create<ContactDetailGetResponse>();
        
        var response = Result<ContactDetailGetResponse>.Success(contactDetail);
        contactDetailService.GetContactDetailByIdAsync(contactId, contactDetailId)
            .Returns(response);

        ActionResult<ApiResponse<ContactDetailGetResponse>> result = await controller.GetContactDetailByIdAsync(contactId, contactDetailId);
        
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetContactDetailByIdAsync_WhenNotFound_Returns_NotFound()
    {
        var contactDetailId = Guid.NewGuid();
        var response = Result<ContactDetailGetResponse>.Failure("Contact detail not found");

        contactDetailService.GetContactDetailByIdAsync(contactId, contactDetailId)
            .Returns(response);

        ActionResult<ApiResponse<ContactDetailGetResponse>> result = await controller.GetContactDetailByIdAsync(contactId, contactDetailId);
        
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task DeleteContactDetailAsync_WhenSuccessful_Return_Ok()
    {
        var contactDetailId = Guid.NewGuid();
        var response = Result<bool>.Success(true);

        contactDetailService.DeleteContactDetailAsync(contactId, contactDetailId)
            .Returns(response);

        ActionResult<ApiResponse<bool>> result = await controller.DeleteContactDetailAsync(contactId, contactDetailId);
        
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task DeleteContactDetailAsync_WhenNotFound_Returns_NotFound()
    {
        var contactDetailId = Guid.NewGuid();
        var response = Result<bool>.Failure("Contact detail not found");

        contactDetailService.DeleteContactDetailAsync(contactId, contactDetailId)
            .Returns(response);

        ActionResult<ApiResponse<bool>> result = await controller.DeleteContactDetailAsync(contactId, contactDetailId);
        
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }
}
