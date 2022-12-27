using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TSITSolutions.ContactSite.Server.Core.Services;

namespace TSITSolutions.ContactSite.Server.MongoDb;

public static class Registration
{    
    public static IServiceCollection AddMongoDbStore(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbOptions>(configuration.GetSection("ProjectsDatabase"));
        services.AddSingleton<IProjectRepository, MongoDbProjectRepository>();
        return services;
    }
    
    public static void InitializeMongoDbStore(this IHost app)
    {
        var options = app.Services.GetRequiredService<IOptionsMonitor<MongoDbOptions>>().CurrentValue;
        var client = new MongoClient(options.ConnectionString);
        var database = client.GetDatabase(options.DatabaseName);
        var collections = database.ListCollectionNames().ToList();
        
        foreach (var collection in MongoSpecs.Collections)
        {
            if (!collections.Contains(collection))
            {
                database.CreateCollection(collection);
            }
        }
    }
}