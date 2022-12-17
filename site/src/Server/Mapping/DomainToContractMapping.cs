using TSITSolutions.ContactSite.Server.Core;
using TSITSolutions.ContactSite.Server.Projects.Contracts;
using TSITSolutions.ContactSite.Shared.Projects;

namespace TSITSolutions.ContactSite.Server.Mapping;

internal static class DomainToContractMapping
{
    internal static ProjectResponse ToContract(this Project project) =>
        new()
        {
            Id = project.Id,
            Title = project.Title,
            Description = project.Description,
            CustomerDomain = project.CustomerDomain,
            IsCurrent = project.EndDate == null,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Technologies = project.Technologies,
        };

    internal static ProjectsResponse ToContract(this IEnumerable<Project> projects) =>
        new()
        {
            Projects = projects.Select(p => p.ToContract())
        };
}