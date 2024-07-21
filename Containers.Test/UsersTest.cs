using Containers.Repository;
using Containers.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace Containers.Test;

public class UsersTest : BaseIntegrationTests
{
    public UsersTest(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Adds_User_To_Database()
    {
        var testUser = new User{ Firstname = "test", Lastname = "name"};

        await DataContext.AddUser(testUser);

        Assert.NotNull(await DataContext.Users.FirstAsync(x => x.Firstname == testUser.Firstname));
    }

}