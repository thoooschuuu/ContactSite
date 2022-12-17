namespace TSITSolutions.ContactSite.Server.Projects.Contracts;

public class ProjectResponse
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public string CustomerDomain { get; init; }
    public bool IsCurrent { get; init; }
    public DateOnly StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
    public IReadOnlyCollection<string> Technologies { get; init; }
}