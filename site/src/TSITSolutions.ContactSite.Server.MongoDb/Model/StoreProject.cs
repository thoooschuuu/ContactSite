using TSITSolutions.ContactSite.Server.Core;

namespace TSITSolutions.ContactSite.Server.MongoDb.Model;

public record StoreProject(
    Guid Id, 
    string Title, 
    string Description, 
    string Role,
    string CustomerDomain,
    DateTime StartDate,
    DateTime? EndDate,
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
            DateOnly.FromDateTime(StartDate), 
            EndDate.HasValue ? DateOnly.FromDateTime(EndDate.Value) : null,
            Technologies
        );
}