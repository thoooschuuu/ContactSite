using FastEndpoints;
using TSITSolutions.ContactSite.Admin.Core.Model;
using TSITSolutions.ContactSite.Admin.Projects.Contracts;

namespace TSITSolutions.ContactSite.Admin.Projects.Mapper;

public class GetProjectListMapper : ResponseMapper<ProjectsResponse, IEnumerable<Project>>
{
    public override ProjectsResponse FromEntity(IEnumerable<Project> p) => new()
    {
        Projects = p.Select(x => new ProjectListResponse
        {
            Id = x.Id,
            Title = x.Title.GetDefaultText(),
            StartDate = x.StartDate,
            EndDate = x.EndDate
        })
    };
}