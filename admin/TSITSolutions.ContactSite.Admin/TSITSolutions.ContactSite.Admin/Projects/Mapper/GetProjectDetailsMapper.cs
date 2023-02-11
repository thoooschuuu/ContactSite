using FastEndpoints;
using TSITSolutions.ContactSite.Admin.Core.Model;
using TSITSolutions.ContactSite.Admin.Projects.Contracts;

namespace TSITSolutions.ContactSite.Admin.Projects.Mapper;

public class GetProjectDetailsMapper : ResponseMapper<ProjectDetailsResponse, Project>
{
    public override ProjectDetailsResponse FromEntity(Project e) => new()
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