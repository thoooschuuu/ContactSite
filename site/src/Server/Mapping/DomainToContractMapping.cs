using Markdig;
using TSITSolutions.ContactSite.Server.Core;
using TSITSolutions.ContactSite.Shared.Projects;

namespace TSITSolutions.ContactSite.Server.Mapping;

internal static class DomainToContractMapping
{
    private static readonly MarkdownPipeline MarkdownPipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
    internal static ProjectResponse ToContract(this Project project) =>
        new()
        {
            Id = project.Id,
            Title = project.Title,
            Description = Markdown.ToHtml(project.Description, MarkdownPipeline).Trim(),
            Role = Markdown.ToHtml(project.Role, MarkdownPipeline).Trim(),
            CustomerDomain = project.CustomerDomain,
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