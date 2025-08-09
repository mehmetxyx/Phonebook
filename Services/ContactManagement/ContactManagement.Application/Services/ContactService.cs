using ContactManagement.Application.Dtos;
using ContactManagement.Application.Interfaces;
using ContactManagement.Application.Mappers;
using ContactManagement.Domain.Repositories;
using Shared.Common;
using Microsoft.Extensions.Logging;

namespace ContactManagement.Application.Services;

public class ContactService: IContactService
{
    private readonly ILogger<ContactService> logger;
    private readonly IUnitOfWork unitOfWork;
    private readonly IContactRepository contactRepository;

    public ContactService(ILogger<ContactService> logger, IUnitOfWork unitOfWork, IContactRepository contactRepository)
    {
        this.logger = logger;
        this.unitOfWork = unitOfWork;
        this.contactRepository = contactRepository;
    }

    public async Task<Result<ContactResponse>> CreateContactAsync(ContactRequest request)
    {
        try
        {
            var contact = request.ToDomain();
            await contactRepository.AddAsync(contact);
            await unitOfWork.SaveAsync();

            return Result<ContactResponse>.Success(contact.ToContactResponse());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating contact");
        }

        return Result<ContactResponse>.Failure("Failed to create contact");
    }

    public async Task<Result<bool>> DeleteContactAsync(Guid contactId)
    {
        try
        {
            var contact = await contactRepository.GetByIdAsync(contactId);
            if(contact == null)
                return Result<bool>.Failure("Contact not found");

            contactRepository.Delete(contact);
            await unitOfWork.SaveAsync();
            
            logger.LogInformation("Contact with ID {ContactId} deleted successfully", contactId);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting contact");
        }

        return Result<bool>.Failure("Failed to delete contact");
    }

    public async Task<Result<ContactResponse>> GetContactByIdAsync(Guid contactId)
    {
        try
        {
            var contact = await contactRepository.GetByIdAsync(contactId);

            if(contact is null)
            {
                logger.LogWarning("Contact with ID {ContactId} not found", contactId);
                return Result<ContactResponse>.Failure("Contact not found");
            }

            return Result<ContactResponse>.Success(contact.ToGetContactResponse());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving contact by ID {ContactId}", contactId);
        }

        return Result<ContactResponse>.Failure("Failed to retrieve contact");
    }

    public async Task<Result<List<ContactResponse>>> GetContactsAsync()
    {
        try
        {
            var contacts = await contactRepository.GetAllAsync();
            
            var responses = contacts
                .Select(c => c.ToGetContactResponse())
                .ToList();

            return Result<List<ContactResponse>>.Success(responses);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving contacts");
        }
     
        return Result<List<ContactResponse>>.Failure("Failed to retrieve contacts");
    }
}