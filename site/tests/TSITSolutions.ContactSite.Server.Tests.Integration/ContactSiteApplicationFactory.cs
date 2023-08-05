using JetBrains.Annotations;
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

[UsedImplicitly]
public sealed class ContactSiteApplicationFactory : WebApplicationFactory<IAssemblyMarker>, IAsyncLifetime
{
    private const string ProjectsDatabaseName = "local";
    public ITestOutputHelper? Output { get; set; }
    private IMongoCollection<StoreProject>? _storeProjectCollection;
    private IMongoCollection<CultureSpecificStoreProject>? _languageSpecificStoreProjectCollection;

    private readonly MongoDbContainer _projectsDatabaseContainer = new MongoDbBuilder()
        .WithUsername("mongo")
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
                ["ProjectsDatabase:ConnectionString"] = _projectsDatabaseContainer.GetConnectionString(),
                ["ProjectsDatabase:DatabaseName"] = ProjectsDatabaseName,
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
        await _projectsDatabaseContainer.StartAsync();
        var client = new MongoClient(_projectsDatabaseContainer.GetConnectionString());
        var database = client.GetDatabase(ProjectsDatabaseName);
        _storeProjectCollection = database.GetCollection<StoreProject>(MongoSpecs.ProjectsCollectionName);
        _languageSpecificStoreProjectCollection = database.GetCollection<CultureSpecificStoreProject>(MongoSpecs.CultureSpecificProjectsCollectionName);
    }

    public new async Task DisposeAsync()
    {
        await _projectsDatabaseContainer.DisposeAsync().AsTask();
    }
    
    
}