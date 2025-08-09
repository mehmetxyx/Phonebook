using AutoFixture;
using ContactManagement.Application.Repositories;
using ContactManagement.Domain.Enums;
using ContactManagement.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactManagement.Infrastructure.Data.Tests;
public class ContactReportDataRepositoryTests
{
    private readonly ContactManagementDbContext context;
    private Fixture fixture;
    private ContactReportDataRepository contactReportDataRepository;
    public ContactReportDataRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ContactManagementDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        context = new ContactManagementDbContext(options);
        fixture = new Fixture();
        contactReportDataRepository = new ContactReportDataRepository(context);
    }

    [Fact]
    public async Task GetContactReportAsync_ShouldReturnContactReport_WhenCalled()
    {
        var contact1 = fixture.Build<ContactEntity>().Without(c => c.ContactDetails).Create();
        var contact2 = fixture.Build<ContactEntity>().Without(c => c.ContactDetails).Create();

        var contactDetail_11 = new ContactDetailEntity
        {
            Id = Guid.NewGuid(),
            ContactId = contact1.Id,
            Type = InformationType.PhoneNumber,
            Value = "pn_11"
        };

        var contactDetail_12 = new ContactDetailEntity
        {
            Id = Guid.NewGuid(),
            ContactId = contact1.Id,
            Type = InformationType.Location,
            Value = "paris"
        };

        var contactDetail_21 = new ContactDetailEntity
        {
            Id = Guid.NewGuid(),
            ContactId = contact2.Id,
            Type = InformationType.Email,
            Value = "21@g.com"
        };

        var contactDetail_22 = new ContactDetailEntity
        {
            Id = Guid.NewGuid(),
            ContactId = contact2.Id,
            Type = InformationType.Location,
            Value = "paris"
        };

        context.Contacts.AddRange(contact1, contact2);
        context.ContactDetails.AddRange(contactDetail_11, contactDetail_12, contactDetail_21, contactDetail_22);
        context.SaveChanges();

        var result = await contactReportDataRepository.GetContactReportDataAsync();

        Assert.NotNull(result);
        Assert.Equal(1, result.Count);
        Assert.Equal("paris", result[0].Location);
        Assert.Equal(2, result[0].ContactCount);
        Assert.Equal(1, result[0].PhoneNumberCount);
    }
}
