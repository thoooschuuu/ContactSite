using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TSITSolutions.ContactSite.Server.Core;
using TSITSolutions.ContactSite.Server.Core.Services;
using TSITSolutions.ContactSite.Server.MongoDb.Model;

namespace TSITSolutions.ContactSite.Server.MongoDb;

public class MongoDbProjectRepository : IProjectRepository
{
    private readonly IMongoCollection<StoreProject> _projectsCollection;

    public MongoDbProjectRepository(IOptionsMonitor<MongoDbOptions> options)
    {
        var client = new MongoClient(options.CurrentValue.ConnectionString);
        var database = client.GetDatabase(options.CurrentValue.DatabaseName);
        _projectsCollection = database.GetCollection<StoreProject>(options.CurrentValue.ProjectsCollectionName);
    }

    public async ValueTask<IEnumerable<Project>> GetAllAsync(CancellationToken ct = default)
    {
        var storeProjects = await _projectsCollection.Find(_ => true).ToListAsync(ct);
        return storeProjects.Select(p => p.ToProject());
    }

    public async ValueTask<Project?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var storeProject = await _projectsCollection.Find(p => p.Id == id).SingleOrDefaultAsync(ct);
        return storeProject.ToProject();
    }
}