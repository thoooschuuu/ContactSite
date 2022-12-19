namespace TSITSolutions.ContactSite.Server.Core;

public record Project(
    Guid Id, 
    string Title, 
    string Description, 
    string Role,
    string CustomerDomain,
    DateOnly StartDate,
    DateOnly? EndDate,
    IReadOnlyCollection<string> Technologies
);