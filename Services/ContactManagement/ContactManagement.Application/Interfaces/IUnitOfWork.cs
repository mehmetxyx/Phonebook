namespace ContactManagement.Application.Interfaces;
public interface IUnitOfWork
{
    Task SaveAsync();
}
