namespace TSITSolutions.ContactSite.Server.MongoDb;

internal static class MongoSpecs
{
    public const string ProjectsCollectionName = "Projects";
    public const string CultureSpecificProjectsCollectionName = "CultureSpecificProjects";

    public static string[] Collections => new[] { ProjectsCollectionName, CultureSpecificProjectsCollectionName };
    
}