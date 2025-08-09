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
        var query = from lcd in context.ContactDetails
                    where lcd.Type == InformationType.Location
                    group lcd by lcd.Value into g
                    select new ContactReportData {
                        Location = g.Key,
                        ContactCount = (from lg in g select lg.ContactId).Distinct().Count(),
                        PhoneNumberCount = (
                            from ic in context.Contacts
                                    join icd in context.ContactDetails on ic.Id equals icd.ContactId
                                    join pcd in context.ContactDetails on ic.Id equals pcd.ContactId
                            where icd.Type == InformationType.Location && icd.Value == g.Key && pcd.Type == InformationType.PhoneNumber
                            select pcd.Value
                        ).Count()
                    };

        return await query.ToListAsync();
    }
}
