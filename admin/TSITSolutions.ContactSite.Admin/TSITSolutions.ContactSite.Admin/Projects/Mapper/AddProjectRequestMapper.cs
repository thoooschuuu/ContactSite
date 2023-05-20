using FastEndpoints;
using TSITSolutions.ContactSite.Admin.Core.Model;
using TSITSolutions.ContactSite.Admin.Projects.Contracts;

namespace TSITSolutions.ContactSite.Admin.Projects.Mapper;

public class AddProjectRequestMapper : Mapper<AddProjectRequest, ProjectDetailsResponse, Project>
{
    public override Project ToEntity(AddProjectRequest r)
    {
        return new(
            Guid.NewGuid(),
            r.Title,
            r.Description ?? MultiLanguageText.Empty,
            r.Role ?? MultiLanguageText.Empty,
            r.CustomerDomain ?? MultiLanguageText.Empty,
            r.StartDate,
            r.EndDate,
            r.Technologies);
    }

    public override ProjectDetailsResponse FromEntity(Project e)
    {
        return new()
        {
            Id = e.Id,
            Title = e.Title,
            Description = e.Description,
            Role = e.Role,
            CustomerDomain = e.CustomerDomain,
            StartDate = e.StartDate,
            EndDate = e.EndDate,
            Technologies = e.Technologies
        };
    }
}