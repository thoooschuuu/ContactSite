using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TSITSolutions.ContactSite.Server.Core.Services;

namespace TSITSolutions.ContactSite.Server.CosmosDb;

public static class Registration
{
    public static IServiceCollection AddCosmosDbStore(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CosmosDbOptions>(configuration.GetSection("CosmosDb"));
        services.AddSingleton<IProjectRepository, CosmosDbProjectRepository>();
        return services;
    }
}

public class CosmosDbOptions
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string ProjectsContainerName { get; set; }
}

