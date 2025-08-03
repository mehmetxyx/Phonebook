using Contact.Domain.DomainEntities;
namespace Contact.Domain.Repositories;

public interface IContactRepository
{
    void Add(ContactDomainEntity contact);
}