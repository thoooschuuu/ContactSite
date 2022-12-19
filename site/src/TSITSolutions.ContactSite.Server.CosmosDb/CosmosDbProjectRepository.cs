using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Options;
using TSITSolutions.ContactSite.Server.Core;
using TSITSolutions.ContactSite.Server.Core.Services;
using TSITSolutions.ContactSite.Server.CosmosDb.Model;

namespace TSITSolutions.ContactSite.Server.CosmosDb;

public class CosmosDbProjectRepository : IProjectRepository
{
    private readonly IOptionsMonitor<CosmosDbOptions> _options;
    private readonly CosmosClient _client;
    
    private Container Container => _client.GetContainer(_options.CurrentValue.DatabaseName, _options.CurrentValue.ProjectsContainerName);

    public CosmosDbProjectRepository(IOptionsMonitor<CosmosDbOptions> options)
    {
        _options = options;
        _client = new CosmosClient(options.CurrentValue.ConnectionString);
    }

    public async ValueTask<IEnumerable<Project>> GetAllAsync(CancellationToken ct = default)
    {
        var query = Container.GetItemQueryIterator<StoreProject>();
        var results = new List<StoreProject>();
        
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync(ct);
            results.AddRange(response.ToList());
        }

        return results.Select(p => p.ToProject());
    }

    public async ValueTask<Project?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var item = await Container.ReadItemAsync<StoreProject>(id.ToString(), new PartitionKey(id.ToString()), cancellationToken: ct);
        
        return item.Resource.ToProject();
    }
}