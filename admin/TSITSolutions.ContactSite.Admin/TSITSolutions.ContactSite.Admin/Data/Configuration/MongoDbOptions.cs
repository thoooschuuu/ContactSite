namespace TSITSolutions.ContactSite.Admin.Data.Configuration;

public class MongoDbOptions
{
    public string ConnectionString { get; set; } = default!;
    public string DatabaseName { get; set; } = default!;
}