using Containers.Repository.Models;

namespace Containers.Test;

public class UsersTest : BaseIntegrationTests
{
    public UsersTest(IntegrationTestWebAppFactory factory) : base(factory)
    {
        DataContext.Users.RemoveRange(DataContext.Users.ToList());
        DataContext.SaveChanges();
        SetupFakeUsers.InitializeFakes(40);
        DataContext.Users.AddRange(SetupFakeUsers.FakeUsers);
        DataContext.SaveChanges();
    }

    [Fact]
    public async Task Adds_Multiple_Users()
    {
        List<User> expectedUsers = [
            new (){ Firstname = "first", Lastname = "name"},
            new (){ Firstname = "second", Lastname = "name"},
            new (){ Firstname = "third", Lastname = "name"}
        ];

        await UserRepository.AddUser(expectedUsers);

        var users = DataContext
            .Users
            .Where(user => expectedUsers.Contains(user))
            .ToList();
        
        Assert.Equivalent(expectedUsers, users);
        Assert.Equal(43, DataContext.Users.ToList().Count);
    }

    [Fact]
    public async Task Adds_User_With_Repository()
    {
        var testUser = new User{ Firstname = "second", Lastname = "test"};
        
        await UserRepository.AddUser(testUser);
        
        Assert.Equivalent(testUser, DataContext.Users.First(x => x.Firstname == testUser.Firstname));
    }

    [Fact]
    public async Task Retrieves_Top_25_Users()
    {
        var mostRecentUsers = await UserRepository.MostRecentUsers();
        
        Assert.Equal(25, mostRecentUsers.Count);
    }

    [Fact]
    public async Task Override_Default_User_Count_When_Retrieving_All_Recent_Users()
    {
        var mostRecentUsers = await UserRepository.MostRecentUsers(2);
        
        Assert.Equal(2, mostRecentUsers.Count);
        Assert.True(mostRecentUsers[0].Id > mostRecentUsers[1].Id);
    }
}