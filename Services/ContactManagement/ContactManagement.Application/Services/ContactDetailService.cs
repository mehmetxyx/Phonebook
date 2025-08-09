using ContactManagement.Application.Dtos;
using ContactManagement.Application.Interfaces;
using ContactManagement.Application.Mappers;
using ContactManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Shared.Common;

namespace ContactManagement.Application.Services;

public class ContactDetailService: IContactDetailService
{
    private ILogger<ContactDetailService> logger;
    private IUnitOfWork unitOfWork;
    private IContactDetailRepository contactDetailRepository;

    public ContactDetailService(ILogger<ContactDetailService> logger, IUnitOfWork unitOfWork, IContactDetailRepository contactDetailRepository)
    {
        this.logger = logger;
        this.unitOfWork = unitOfWork;
        this.contactDetailRepository = contactDetailRepository;
    }

    public async Task<Result<ContactDetailResponse>> CreateContactDetailAsync(Guid contactId, ContactDetailRequest request)
    {
        try
        {
            var contactDetail = request.ToDomain(contactId);
            
            await contactDetailRepository.AddAsync(contactDetail);
            await unitOfWork.SaveAsync();

            return Result<ContactDetailResponse>.Success(contactDetail.ToContactDetailResponse());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating contact detail for contact {ContactId}", contactId);
        }

        return Result<ContactDetailResponse>.Failure("Failed to create contact detail");
    }

    public async Task<Result<List<ContactDetailResponse>>> GetAllContactDetailsAsync(Guid contactId)
    {
        try
        {
            var contactDetails = await contactDetailRepository.GetAllAsync(contactId);

            var responses = contactDetails
                .Select(cd => cd.ToContactDetailResponse())
                .ToList();

            return Result<List<ContactDetailResponse>>.Success(responses);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving contact details for contact {ContactId}", contactId);
        }

        return Result<List<ContactDetailResponse>>.Failure("Failed to retrieve contact details");
    }

    public async Task<Result<ContactDetailResponse>> GetContactDetailByIdAsync(Guid contactId, Guid contactDetailId)
    {
        try
        {
            var contactDetail = await contactDetailRepository.GetByIdAsync(contactId, contactDetailId);
            if (contactDetail == null)
            {
                return Result<ContactDetailResponse>.Failure("Contact detail not found");
            }

            return Result<ContactDetailResponse>.Success(contactDetail.ToContactDetailResponse());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving contact detail {ContactDetailId} for contact {ContactId}", contactDetailId, contactId);
        }

        return Result<ContactDetailResponse>.Failure("Failed to retrieve contact detail");
    }


    public async Task<Result<bool>> DeleteContactDetailAsync(Guid contactId, Guid contactDetailId)
    {
        try
        {
            var contactDetail = await contactDetailRepository.GetByIdAsync(contactId, contactDetailId);
            if (contactDetail == null)
            {
                return Result<bool>.Failure("Contact detail not found");
            }

            contactDetailRepository.Delete(contactDetail);
            await unitOfWork.SaveAsync();
            
            logger.LogInformation("Contact detail with ID {ContactDetailId} for contact {ContactId} deleted successfully", contactDetailId, contactId);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting contact detail {ContactDetailId} for contact {ContactId}", contactDetailId, contactId);
        }

        return Result<bool>.Failure("Failed to delete contact detail");
    }

}