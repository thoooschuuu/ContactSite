using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Testcontainers.MongoDb;
using TSITSolutions.ContactSite.Server.MongoDb;
using TSITSolutions.ContactSite.Server.MongoDb.Model;
using Xunit.Abstractions;

namespace TSITSolutions.ContactSite.Server.Tests.Integration;

public class ContactSiteApplicationFactory : WebApplicationFactory<IAssemblyMarker>, IAsyncLifetime
{
    public ITestOutputHelper? Output { get; set; }
    private IMongoCollection<StoreProject>? _storeProjectCollection;
    private IMongoCollection<CultureSpecificStoreProject>? _languageSpecificStoreProjectCollection;

    private readonly MongoDbContainer _projectsDatabase = new MongoDbBuilder()
        .WithUsername("mongo")
        .WithPassword("mongo")
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
                ["ProjectsDatabase:ConnectionString"] = _projectsDatabase.GetConnectionString(),
                ["ProjectsDatabase:DatabaseName"] = _projectsDatabase.Name,
                ["ProjectsDatabase:CollectionName"] = "Projects",
                ["Caching:Enabled"] = "false"
            });
        });
    }
    
    public async Task AddProject(StoreProject project)
    {
        await _storeProjectCollection!.InsertOneAsync(project);
    }
    
    public async Task AddCultureSpecificProject(CultureSpecificStoreProject project)
    {
        await _languageSpecificStoreProjectCollection!.InsertOneAsync(project);
    }
    
    public async Task ClearCollections()
    {
        await _storeProjectCollection!.DeleteManyAsync(FilterDefinition<StoreProject>.Empty);
        await _languageSpecificStoreProjectCollection!.DeleteManyAsync(FilterDefinition<CultureSpecificStoreProject>.Empty);
    }

    public async Task InitializeAsync()
    {
        await _projectsDatabase.StartAsync();
        var client = new MongoClient(_projectsDatabase.GetConnectionString());
        var database = client.GetDatabase(_projectsDatabase.Name);
        _storeProjectCollection = database.GetCollection<StoreProject>(MongoSpecs.ProjectsCollectionName);
        _languageSpecificStoreProjectCollection = database.GetCollection<CultureSpecificStoreProject>(MongoSpecs.CultureSpecificProjectsCollectionName);
    }

    public new async Task DisposeAsync()
    {
        await _projectsDatabase.StopAsync();
    }
    
    
}