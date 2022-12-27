using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TSITSolutions.ContactSite.Server.Core;
using TSITSolutions.ContactSite.Server.Core.Services;
using TSITSolutions.ContactSite.Server.MongoDb.Model;

namespace TSITSolutions.ContactSite.Server.MongoDb;

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

    public async ValueTask<IEnumerable<Project>> GetAllAsync(string? culture = null, CancellationToken ct = default)
    {
        var storeProjects = await _projectsCollection.Find(_ => true).ToListAsync(ct);
        var cultureSpecificStoreProjects = 
            await _cultureSpecificProjectsCollection
                .Find(p => p.Culture.Equals(culture))
                .ToListAsync(ct)
            ?? new List<CultureSpecificStoreProject>();
        
        CultureSpecificStoreProject? GetById(Guid id) => cultureSpecificStoreProjects.FirstOrDefault(p => p.ProjectId == id);
        
        return storeProjects.Select(p => p.ToProject(GetById(p.Id)));
    }

    public async ValueTask<Project> GetByIdAsync(Guid id, string? culture = null, CancellationToken ct = default)
    {
        var storeProject = await _projectsCollection.Find(p => p.Id == id).SingleOrDefaultAsync(ct);
        if(storeProject is null)
        {
            return Project.Empty;
        }
        var cultureOverride = await _cultureSpecificProjectsCollection.Find(p => p.ProjectId == id && p.Culture.Equals(culture)).SingleOrDefaultAsync(ct);
        return storeProject.ToProject(cultureOverride);
    }
}