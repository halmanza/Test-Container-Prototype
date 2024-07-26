using Containers.Repository;
using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace Containers.Test;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private const string Database = "master";
    private const string Password = "learning123!";
    private const string Username = "sa";
    private const ushort MsSqlPort = 1433;
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
        .WithEnvironment("ACCEPT_EULA", "Y")
        .WithEnvironment("MSSQL_SA_PASSWORD", Password)
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(MsSqlPort))
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var host = _msSqlContainer.Hostname;
        var port = _msSqlContainer.GetMappedPublicPort(MsSqlPort);

        var connectionString =$"Server={host},{port};Database={Database};User Id={Username};Password={Password};TrustServerCertificate=True";
        
        builder.ConfigureTestServices(services =>
        {
            var descriptor =
                services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<ApplicationDataContext>));

            if (descriptor is not null)
                services.Remove(descriptor);

            services.AddDbContext<ApplicationDataContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

        });
    }
    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _msSqlContainer.StopAsync();
    }
}