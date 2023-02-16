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
)
{
    public Project ToProject(params CultureSpecificStoreProject[] projectOverrides) =>
        new(
            Id,
            GetMultiLanguageText(Title, p => p.Title, projectOverrides),
            GetMultiLanguageText(Description, p => p.Description, projectOverrides),
            GetMultiLanguageText(Role, p => p.Role, projectOverrides),
            GetMultiLanguageText(CustomerDomain, p => p.CustomerDomain, projectOverrides),
            DateOnly.FromDateTime(StartDate), 
            EndDate.HasValue ? DateOnly.FromDateTime(EndDate.Value) : null,
            Technologies
        );

    private MultiLanguageText GetMultiLanguageText(string? text, Func<CultureSpecificStoreProject, string> selector, params CultureSpecificStoreProject[] projectOverrides)
    {
        var overrides = projectOverrides.Select(p => (p.Culture, selector(p)));
        return MultiLanguageText.Create(overrides.Append(("de-DE", text)).ToArray());
    }
        
}