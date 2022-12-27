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
)
{
    public static Project Empty => new(Guid.Empty, string.Empty, string.Empty, string.Empty, string.Empty, default, default, Array.Empty<string>());
};