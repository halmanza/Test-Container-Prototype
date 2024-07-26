using Containers.Repository.Models;

namespace Containers.Repository.Interfaces;

public interface IUserRepository
{
    Task AddUser(User user);
    Task AddUser(List<User> users);
    Task<List<User>> MostRecentUsers(int amountOfUsers = 25);
}