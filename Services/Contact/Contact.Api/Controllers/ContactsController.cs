using Contact.Application.Dtos;
using Contact.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Api.Common;

namespace Contact.Api.Controllers;
[ApiController]
[Route("api/contacts")]
public class ContactsController : ControllerBase
{
    private readonly ILogger<ContactsController> _logger;
    private IContactService contactService;

    public ContactsController(ILogger<ContactsController> logger, IContactService contactService)
    {
        this.contactService = contactService;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ContactCreateResponse>>> CreateContactAsync(ContactCreateRequest request)
    {
        var result = contactService.CreateContactAsync(request);
        if(result.IsSuccess)
            return CreatedAtAction(nameof(CreateContactAsync), new { id = result.Value.Id }, result.ToApiResponse());   

        return BadRequest(result.ToApiResponse());
    }

    [HttpDelete("{contactId}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteContactAsync(Guid contactId)
    {
        var result = contactService.DeleteContactAsync(contactId);
        if (result.IsSuccess)
            return Ok(result.ToApiResponse());
        return NotFound(result.ToApiResponse());
    }

    [HttpGet("{contactId}")]
    public async Task<ActionResult<ApiResponse<GetContactResponse>>> GetContactAsync(Guid contactId)
    {
        var result = contactService.GetContactAsync(contactId);

        if(result.IsSuccess)
            return Ok(result.ToApiResponse());

        return NotFound(result.ToApiResponse());
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<GetContactResponse>>>> GetContactsAsync()
    {
        var result = contactService.GetContactsAsync();
        if (result.IsSuccess)
            return Ok(result.ToApiResponse()); 
        
        return NotFound(result.ToApiResponse());
    }
}
