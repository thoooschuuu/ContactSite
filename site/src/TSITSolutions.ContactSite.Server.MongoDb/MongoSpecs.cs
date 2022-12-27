namespace TSITSolutions.ContactSite.Server.MongoDb;

internal static class MongoSpecs
{
    public const string ProjectsCollectionName = "Projects";
    public const string LanguageSpecificProjectsCollectionName = "LanguageSpecificProjects";

    public static string[] Collections => new[] { ProjectsCollectionName, LanguageSpecificProjectsCollectionName };
    
}