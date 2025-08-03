using ContactManagement.Application.Dtos;
using ContactManagement.Application.Interfaces;
using ContactManagement.Application.Services;
using ContactManagement.Domain.Repositories;
using NSubstitute;

namespace ContactManagement.Application.Tests;

public class ContactServiceTests
{
    private readonly IContactRepository contactRepository;
    private readonly IUnitOfWork unitOfWork;
    public ContactServiceTests()
    {
        contactRepository = Substitute.For<IContactRepository>();
        unitOfWork = Substitute.For<IUnitOfWork>();
    }
    [Fact]
    public void Test1()
    {
        var contactService = new ContactService(unitOfWork, contactRepository);

        var result = contactService.CreateContactAsync(new ContactCreateRequest());

        Assert.True(result.IsSuccess);
    }
}
