using ContactManagement.Application;
using ContactManagement.Application.Dtos;
using ContactManagement.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Api.Common;

namespace ContactManagement.Api.Controllers;
[Route("api/contacts/{contactId}/contact-details")]
[ApiController]
public class ContactDetailsController : ControllerBase
{
    private readonly IContactDetailService contactDetailService;

    public ContactDetailsController(IContactDetailService contactDetailService)
    {
        this.contactDetailService = contactDetailService;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ContactDetailCreateResponse>>> ContactDetailCreateAsync(Guid contactId, ContactDetailCreateRequest createRequest)
    {
        var result = await contactDetailService.CreateContactDetailAsync(contactId, createRequest);

        if (result.IsSuccess)
            return CreatedAtAction(nameof(ContactDetailCreateAsync), new { id = result?.Value?.Id }, result?.ToApiResponse());

        return BadRequest(result.ToApiResponse());
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<ContactDetailGetResponse>>>> GetAllContactDetailsAsync(Guid contactId)
    {
        var result = await contactDetailService.GetAllContactDetailsAsync(contactId);

        if (result.IsSuccess)
            return Ok(result.ToApiResponse());
        return NotFound(result.ToApiResponse());
    }

    [HttpGet("{contactDetailId}")]
    public async Task<ActionResult<ApiResponse<ContactDetailGetResponse>>> GetContactDetailByIdAsync(Guid contactId, Guid contactDetailId)
    {
        var result = await contactDetailService.GetContactDetailByIdAsync(contactId, contactDetailId);
        if (result.IsSuccess)
            return Ok(result.ToApiResponse());

        return NotFound(result.ToApiResponse());
    }

    [HttpPut("{contactDetailId}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteContactDetailAsync(Guid contactId, Guid contactDetailId)
    {
        var result = await contactDetailService.DeleteContactDetailAsync(contactId, contactDetailId);
        if (result.IsSuccess)
            return Ok(result.ToApiResponse());
        
        return NotFound(result.ToApiResponse());
    }

}
