using JetBrains.Annotations;

namespace TSITSolutions.ContactSite.Shared.Projects;

[PublicAPI]
public class ProjectResponse
{
    public Guid Id { get; init; }
    public string Title { get; init; } = default!;
    public string Description { get; init; } = default!;
    public string Role { get; set; } = default!;
    public string CustomerDomain { get; init; } = default!;
    public DateOnly StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
    public IReadOnlyCollection<string> Technologies { get; init; } = Array.Empty<string>();
}