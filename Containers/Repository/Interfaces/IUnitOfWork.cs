namespace Containers.Repository.Interfaces;

public interface IUnitOfWork
{
    Task Save();
}