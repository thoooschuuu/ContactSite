using JetBrains.Annotations;

namespace TSITSolutions.ContactSite.Admin.Projects.Contracts;

[PublicAPI]
public class ProjectListResponse
{
    public Guid Id { get; init; }
    public string Title { get; init; } = default!;
    public DateOnly StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
}