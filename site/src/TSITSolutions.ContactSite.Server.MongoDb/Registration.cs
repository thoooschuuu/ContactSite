using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TSITSolutions.ContactSite.Server.Core.Services;

namespace TSITSolutions.ContactSite.Server.MongoDb;

public static class Registration
{
    public static IServiceCollection AddMongoDbStore(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbOptions>(configuration.GetSection("Database"));
        services.AddSingleton<IProjectRepository, MongoDbProjectRepository>();
        return services;
    }
}

public class MongoDbOptions
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string ProjectsCollectionName { get; set; }
}

