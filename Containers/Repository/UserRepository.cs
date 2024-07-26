using Containers.Repository.Interfaces;
using Containers.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace Containers.Repository;

public class UserRepository(ApplicationDataContext dataContext) : IUserRepository, IUnitOfWork
{
    public async Task AddUser(User user)
    {
        await dataContext.Users.AddAsync(user);
        await dataContext.SaveChangesAsync();
    }

    public async Task AddUser(List<User> users)
    {
        await dataContext.Users.AddRangeAsync(users);
        await dataContext.SaveChangesAsync();
    }

    public async Task<List<User>> MostRecentUsers(int amountOfUsers = 25)
    {
        return await dataContext
            .Users
            .OrderByDescending(x => x.Id)
            .Take(amountOfUsers)
            .ToListAsync();
    }

    public async Task Save()
    {
        await dataContext.SaveChangesAsync();
    }
}