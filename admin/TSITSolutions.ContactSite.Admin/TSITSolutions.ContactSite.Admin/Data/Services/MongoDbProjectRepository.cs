using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TSITSolutions.ContactSite.Admin.Core.Model;
using TSITSolutions.ContactSite.Admin.Core.Services;
using TSITSolutions.ContactSite.Admin.Data.Configuration;
using TSITSolutions.ContactSite.Admin.Data.Model;

namespace TSITSolutions.ContactSite.Admin.Data.Services;

public class MongoDbProjectRepository : IProjectRepository
{
    private readonly IMongoCollection<StoreProject> _projectsCollection;
    private readonly IMongoCollection<CultureSpecificStoreProject> _cultureSpecificProjectsCollection;

    public MongoDbProjectRepository(IOptionsMonitor<MongoDbOptions> options)
    {
        var client = new MongoClient(options.CurrentValue.ConnectionString);
        var database = client.GetDatabase(options.CurrentValue.DatabaseName);

        _projectsCollection = database.GetCollection<StoreProject>(MongoSpecs.ProjectsCollectionName);
        _cultureSpecificProjectsCollection = database.GetCollection<CultureSpecificStoreProject>(MongoSpecs.CultureSpecificProjectsCollectionName);
    }

    public async ValueTask<IEnumerable<Project>> GetAllAsync(CancellationToken ct = default)
    {
        var storeProjects = await _projectsCollection.Find(_ => true).ToListAsync(ct);
        return storeProjects.Select(p => p.ToProject());
    }

    public async ValueTask<Project> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var storeProject = await _projectsCollection.Find(p => p.Id == id).SingleOrDefaultAsync(ct);
        if(storeProject is null)
        {
            return Project.Empty;
        }
        var cultureOverride = await _cultureSpecificProjectsCollection.Find(p => p.ProjectId == id).SingleOrDefaultAsync(ct);
        return storeProject.ToProject(cultureOverride);
    }
}