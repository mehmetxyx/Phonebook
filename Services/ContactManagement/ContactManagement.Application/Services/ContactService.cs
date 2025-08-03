using ContactManagement.Application.Dtos;
using ContactManagement.Application.Interfaces;
using ContactManagement.Domain.Repositories;
using ContactManagement.Application.Dtos;
using Shared.Common;

namespace ContactManagement.Application.Services;

public class ContactService: IContactService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IContactRepository contactRepository;

    public ContactService(IUnitOfWork unitOfWork, IContactRepository contactRepository)
    {
        this.unitOfWork = unitOfWork;
        this.contactRepository = contactRepository;
    }

    public Result<ContactCreateResponse> CreateContactAsync(ContactCreateRequest request)
    {
        var contact = request.ToDomain();
        contactRepository.Add(contact);
        unitOfWork.SaveAsync();

        return Result<ContactCreateResponse>.Success(contact.ToResponse());
    }

    public Result<bool> DeleteContactAsync(Guid contactId)
    {
        throw new NotImplementedException();
    }

    public Result<GetContactResponse> GetContactAsync(Guid contactId)
    {
        throw new NotImplementedException();
    }

    public Result<List<GetContactResponse>> GetContactsAsync()
    {
        throw new NotImplementedException();
    }
}