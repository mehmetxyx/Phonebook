using Contact.Application.Dtos;
using Contact.Application.Interfaces;
using Contact.Application.Services;
using Contact.Domain.Repositories;
using NSubstitute;

namespace Contact.Application.Tests;

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
