using ContactManagement.Application.Dtos;
using ContactManagement.Domain.Enums;
using ContactManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ContactManagement.Application.Repositories;
public class ContactReportDataRepository : IContactReportDataRepository
{
    private ContactManagementDbContext context;

    public ContactReportDataRepository(ContactManagementDbContext context)
    {
        this.context = context;
    }

    public async Task<List<ContactReportData>> GetContactReportDataAsync()
    {
        var query = context.ContactDetails
            .Where(cd => cd.Type == InformationType.Location)
            .GroupBy(cd => cd.Value)
            .Select(g => new ContactReportData
            {
                Location = g.Key,
                ContactCount = g.Select(cd => cd.ContactId).Distinct().Count(),
                PhoneNumberCount = context.ContactDetails.Count(pc => pc.Type == InformationType.PhoneNumber && pc.Value == g.Key),
            });

        return await query.ToListAsync();
    }
}
