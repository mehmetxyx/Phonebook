using ContactManagement.Application.Dtos;
using ContactManagement.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Api.Common;

namespace ContactManagement.Api.Controllers;
[ApiController]
[Route("api/contacts")]
public class ContactsController : ControllerBase
{
    private IContactService contactService;

    public ContactsController(IContactService contactService)
    {
        this.contactService = contactService;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ContactResponse>>> CreateContactAsync(ContactRequest request)
    {
        var result = await contactService.CreateContactAsync(request);
        if(result.IsSuccess)
            return CreatedAtAction(nameof(CreateContactAsync), new { id = result?.Value?.Id }, result?.ToApiResponse());   

        return BadRequest(result.ToApiResponse());
    }

    [HttpDelete("{contactId}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteContactAsync(Guid contactId)
    {
        var result = await contactService.DeleteContactAsync(contactId);
        if (result.IsSuccess)
            return Ok(result.ToApiResponse());
        return NotFound(result.ToApiResponse());
    }

    [HttpGet("{contactId}")]
    public async Task<ActionResult<ApiResponse<ContactResponse>>> GetContactByIdAsync(Guid contactId)
    {
        var result = await contactService.GetContactByIdAsync(contactId);

        if(result.IsSuccess)
            return Ok(result.ToApiResponse());

        return NotFound(result.ToApiResponse());
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<ContactResponse>>>> GetContactsAsync()
    {
        var result = await contactService.GetContactsAsync();
        if (result.IsSuccess)
            return Ok(result.ToApiResponse()); 
        
        return NotFound(result.ToApiResponse());
    }
}
