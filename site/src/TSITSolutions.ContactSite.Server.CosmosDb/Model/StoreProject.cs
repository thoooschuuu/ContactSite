using TSITSolutions.ContactSite.Server.Core;

namespace TSITSolutions.ContactSite.Server.CosmosDb.Model;

public record StoreProject(
    Guid Id, 
    string Title, 
    string Description, 
    string Role,
    string CustomerDomain,
    DateOnly StartDate,
    DateOnly? EndDate,
    IReadOnlyCollection<string> Technologies
)
{
    public Project ToProject() =>
        new(
            Id,
            Title,
            Description,
            Role,
            CustomerDomain,
            StartDate,
            EndDate,
            Technologies
        );
}