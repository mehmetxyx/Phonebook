using ContactManagement.Domain.DomainEntities;
namespace ContactManagement.Domain.Repositories;

public interface IContactRepository
{
    void Add(Contact contact);
}