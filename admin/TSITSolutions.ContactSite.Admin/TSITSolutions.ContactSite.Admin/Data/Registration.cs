using TSITSolutions.ContactSite.Admin.Core.Services;
using TSITSolutions.ContactSite.Admin.Data.Configuration;
using TSITSolutions.ContactSite.Admin.Data.Services;

namespace TSITSolutions.ContactSite.Admin.Data;

public static class Registration
{    
    public static IServiceCollection AddMongoDbStore(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbOptions>(configuration.GetSection("ProjectsDatabase"));
        services.AddSingleton<IProjectRepository, MongoDbProjectRepository>();
        return services;
    }
}