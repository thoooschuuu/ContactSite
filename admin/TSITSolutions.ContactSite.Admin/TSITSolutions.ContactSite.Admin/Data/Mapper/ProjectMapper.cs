using TSITSolutions.ContactSite.Admin.Core.Model;
using TSITSolutions.ContactSite.Admin.Data.Model;

namespace TSITSolutions.ContactSite.Admin.Data.Mapper;

public static class ProjectMapper
{
    public static Project ToProject(this StoreProject project, params CultureSpecificStoreProject[] projectOverrides) =>
        new(
            project.Id,
            GetMultiLanguageText(project.Title, p => p.Title, projectOverrides),
            GetMultiLanguageText(project.Description, p => p.Description, projectOverrides),
            GetMultiLanguageText(project.Role, p => p.Role, projectOverrides),
            GetMultiLanguageText(project.CustomerDomain, p => p.CustomerDomain, projectOverrides),
            DateOnly.FromDateTime(project.StartDate), 
            project.EndDate.HasValue ? DateOnly.FromDateTime(project.EndDate.Value) : null,
            project.Technologies
        );

    private static MultiLanguageText GetMultiLanguageText(string? text, Func<CultureSpecificStoreProject, string> selector, params CultureSpecificStoreProject[] projectOverrides)
    {
        var overrides = projectOverrides.Select(p => (p.Culture, selector(p)));
        return MultiLanguageText.Create(overrides.Append(("de-DE", text)).ToArray());
    }

    public static StoreProject ToStoreProject(this Project project) =>
        new StoreProject(
            project.Id,
            project.Title.GetDefaultText(),
            project.Description.GetDefaultText(),
            project.Role.GetDefaultText(),
            project.CustomerDomain.GetDefaultText(),
            project.StartDate.ToDateTime(TimeOnly.MinValue),
            project.EndDate?.ToDateTime(TimeOnly.MinValue),
            project.Technologies
        );

    public static CultureSpecificStoreProject ToCultureSpecificStoreProject(this Project project, string culture) =>
        new(
            Guid.NewGuid(),
            project.Id,
            project.Title[culture],
            project.Description[culture],
            project.Role[culture],
            project.CustomerDomain[culture],
            culture
        );
}