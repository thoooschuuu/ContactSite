using TSITSolutions.ContactSite.Admin.Core.Model;

namespace TSITSolutions.ContactSite.Admin.Projects.Contracts;

public record AddProjectRequest(
        MultiLanguageText Title,
        MultiLanguageText? Description,
        MultiLanguageText? Role,
        MultiLanguageText? CustomerDomain,
        DateOnly StartDate,
        DateOnly? EndDate,
        IReadOnlyCollection<string> Technologies
    );