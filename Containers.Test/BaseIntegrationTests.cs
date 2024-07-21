using Containers.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Containers.Test;

public abstract class BaseIntegrationTests : IClassFixture<IntegrationTestWebAppFactory>
{
    protected readonly ApplicationDataContext DataContext;

    protected BaseIntegrationTests(IntegrationTestWebAppFactory factory)
    {
        var scope = factory.Services.CreateScope();

        DataContext = scope.ServiceProvider.GetRequiredService<ApplicationDataContext>();
        DataContext.Database.Migrate();
    }
}