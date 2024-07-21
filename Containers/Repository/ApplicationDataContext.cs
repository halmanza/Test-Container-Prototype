using Containers.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace Containers.Repository;

public class ApplicationDataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDataContext).Assembly);
    }

    public async Task AddUser(User user)
    {
        await Users.AddAsync(user);
        await SaveChangesAsync();
    }
}
