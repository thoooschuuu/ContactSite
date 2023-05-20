using JetBrains.Annotations;
using TSITSolutions.ContactSite.Admin.Core.Model;

namespace TSITSolutions.ContactSite.Admin.Projects.Contracts;

[PublicAPI]
public class ProjectDetailsResponse
{
    public Guid Id { get; init; }
    public MultiLanguageText Title { get; init; } = default!;
    public MultiLanguageText Description { get; init; } = default!;
    public MultiLanguageText Role { get; set; } = default!;
    public MultiLanguageText CustomerDomain { get; init; } = default!;
    public DateOnly StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
    public IReadOnlyCollection<string> Technologies { get; init; } = Array.Empty<string>();
}