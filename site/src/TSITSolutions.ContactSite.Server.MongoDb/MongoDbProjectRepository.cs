using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TSITSolutions.ContactSite.Server.Core;
using TSITSolutions.ContactSite.Server.Core.Services;
using TSITSolutions.ContactSite.Server.MongoDb.Model;

namespace TSITSolutions.ContactSite.Server.MongoDb;

public class MongoDbProjectRepository : IProjectRepository
{
    private readonly IMongoCollection<StoreProject> _projectsCollection;
    private readonly IMongoCollection<LanguageSpecificStoreProject> _languageSpecificProjectsCollection;

    public MongoDbProjectRepository(IOptionsMonitor<MongoDbOptions> options)
    {
        var client = new MongoClient(options.CurrentValue.ConnectionString);
        var database = client.GetDatabase(options.CurrentValue.DatabaseName);

        _projectsCollection = database.GetCollection<StoreProject>(MongoSpecs.ProjectsCollectionName);
        _languageSpecificProjectsCollection = database.GetCollection<LanguageSpecificStoreProject>(MongoSpecs.LanguageSpecificProjectsCollectionName);
    }

    public async ValueTask<IEnumerable<Project>> GetAllAsync(string? language = null, CancellationToken ct = default)
    {
        var storeProjects = await _projectsCollection.Find(_ => true).ToListAsync(ct);
        var languageSpecificStoreProjects = 
            await _languageSpecificProjectsCollection
                .Find(p => p.Language.Equals(language))
                .ToListAsync(ct)
            ?? new List<LanguageSpecificStoreProject>();
        
        LanguageSpecificStoreProject? GetById(Guid id) => languageSpecificStoreProjects.FirstOrDefault(p => p.ProjectId == id);
        
        return storeProjects.Select(p => p.ToProject(GetById(p.Id)));
    }

    public async ValueTask<Project> GetByIdAsync(Guid id, string? language = null, CancellationToken ct = default)
    {
        var storeProject = await _projectsCollection.Find(p => p.Id == id).SingleOrDefaultAsync(ct);
        if(storeProject is null)
        {
            return Project.Empty;
        }
        var languageOverride = await _languageSpecificProjectsCollection.Find(p => p.ProjectId == id).SingleOrDefaultAsync(ct);
        return storeProject.ToProject(languageOverride);
    }
}