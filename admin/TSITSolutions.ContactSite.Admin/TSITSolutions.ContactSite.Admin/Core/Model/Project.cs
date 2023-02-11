namespace TSITSolutions.ContactSite.Admin.Core.Model;

public record Project(
    Guid Id,
    MultiLanguageText Title,
    MultiLanguageText Description,
    MultiLanguageText Role,
    MultiLanguageText CustomerDomain,
    DateOnly StartDate,
    DateOnly? EndDate,
    IReadOnlyCollection<string> Technologies
)
{
    public static Project Empty => new(
        Guid.Empty, 
        MultiLanguageText.Empty, 
        MultiLanguageText.Empty, 
        MultiLanguageText.Empty, 
        MultiLanguageText.Empty, 
        default, 
        default, 
        Array.Empty<string>());
};