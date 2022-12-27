namespace TSITSolutions.ContactSite.Server.MongoDb.Model;

public record LanguageSpecificStoreProject(
    Guid Id,
    Guid ProjectId,
    string Title,
    string Description,
    string Role,
    string CustomerDomain,
    string Language
);