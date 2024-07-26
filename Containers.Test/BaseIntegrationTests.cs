using Bogus;
using Containers.Repository;
using Containers.Repository.Interfaces;
using Containers.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Containers.Test;

public abstract class BaseIntegrationTests : IClassFixture<IntegrationTestWebAppFactory>
{
    protected readonly ApplicationDataContext DataContext;
    protected readonly IUserRepository UserRepository;

    protected BaseIntegrationTests(IntegrationTestWebAppFactory factory)
    {
        var scope = factory.Services.CreateScope();

        DataContext = scope.ServiceProvider.GetRequiredService<ApplicationDataContext>();
        DataContext.Database.Migrate();
        UserRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        
    }
}

public static class SetupFakeUsers
{
    public static List<User> FakeUsers = new();

    public static void InitializeFakes(int amount)
    {
        FakeUsers.Clear();
        var fakeUser = new Faker<User>()
            .RuleFor(x => x.Firstname, f => f.Name.FirstName())
            .RuleFor(x => x.Lastname, f => f.Name.LastName());
        var fakeUsers = fakeUser.Generate(amount);
        
        FakeUsers.AddRange(fakeUsers);
    }
}    
