namespace TSITSolutions.ContactSite.Admin.Data.Model;

public record CultureSpecificStoreProject(
    Guid Id,
    Guid ProjectId,
    string Title,
    string Description,
    string Role,
    string CustomerDomain,
    string Culture
);