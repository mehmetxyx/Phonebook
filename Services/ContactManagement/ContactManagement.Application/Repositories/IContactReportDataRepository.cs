using ContactManagement.Application.Dtos;

namespace ContactManagement.Application.Repositories;

public interface IContactReportDataRepository
{
    Task<List<ContactReportData>> GetContactReportDataAsync();
}
