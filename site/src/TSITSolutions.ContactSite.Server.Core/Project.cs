namespace TSITSolutions.ContactSite.Server.Core;

public record Project(
    Guid Id, 
    string Title, 
    string Description, 
    string CustomerDomain,
    bool IsCurrent,
    DateOnly StartDate,
    DateOnly? EndDate,
    IReadOnlyCollection<string> Technologies
);