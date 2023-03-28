using TSITSolutions.ContactSite.Admin.Core.Model;

namespace TSITSolutions.ContactSite.Admin.Data.Model;

public record StoreProject(
    Guid Id, 
    string Title, 
    string Description, 
    string Role,
    string CustomerDomain,
    DateTime StartDate,
    DateTime? EndDate,
    IReadOnlyCollection<string> Technologies
);