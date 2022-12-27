using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using TSITSolutions.ContactSite.Server.MongoDb.Model;
using Xunit.Abstractions;

namespace TSITSolutions.ContactSite.Server.Tests.Integration;

public class ContactSiteApplicationFactory : WebApplicationFactory<IAssemblyMarker>, IAsyncLifetime
{
    public ITestOutputHelper? Output { get; set; }
    private IMongoCollection<StoreProject>? _storeProjectCollection;
    private IMongoCollection<LanguageSpecificStoreProject>? _languageSpecificStoreProjectCollection;

    private readonly TestcontainerDatabase _projectsDatabase = new TestcontainersBuilder<MongoDbTestcontainer>()
        .WithDatabase(new MongoDbTestcontainerConfiguration
        {
            Database = "db",
            Username = "mongo",
            Password = "mongo",
        })
        .Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            if (Output != null)
            {
                logging.AddXUnit(Output);
            }
        });
        
        builder.ConfigureAppConfiguration(configBuilder =>
        {
            configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ProjectsDatabase:ConnectionString"] = _projectsDatabase.ConnectionString,
                ["ProjectsDatabase:DatabaseName"] = _projectsDatabase.Database,
                ["ProjectsDatabase:CollectionName"] = "Projects"
            });
        });
    }
    
    public async Task AddProject(StoreProject project)
    {
        await _storeProjectCollection!.InsertOneAsync(project);
    }
    
    public async Task AddLanguageSpecificProject(LanguageSpecificStoreProject project)
    {
        await _languageSpecificStoreProjectCollection!.InsertOneAsync(project);
    }
    
    public async Task ClearCollections()
    {
        await _storeProjectCollection!.DeleteManyAsync(FilterDefinition<StoreProject>.Empty);
        await _languageSpecificStoreProjectCollection!.DeleteManyAsync(FilterDefinition<LanguageSpecificStoreProject>.Empty);
    }

    public async Task InitializeAsync()
    {
        await _projectsDatabase.StartAsync();
        var client = new MongoClient(_projectsDatabase.ConnectionString);
        var database = client.GetDatabase(_projectsDatabase.Database);
        _storeProjectCollection = database.GetCollection<StoreProject>("Projects");
        _languageSpecificStoreProjectCollection = database.GetCollection<LanguageSpecificStoreProject>("LanguageSpecificProjects");
    }

    public new async Task DisposeAsync()
    {
        await _projectsDatabase.StopAsync();
    }
    
    
}