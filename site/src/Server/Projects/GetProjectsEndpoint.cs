using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using TSITSolutions.ContactSite.Server.Core.Services;
using TSITSolutions.ContactSite.Server.Mapping;
using TSITSolutions.ContactSite.Server.Projects.Contracts;

namespace TSITSolutions.ContactSite.Server.Projects;

[HttpGet("/api/projects")]
[AllowAnonymous]
public class GetProjectsEndpoint : EndpointWithoutRequest<ProjectsResponse>
{
    private readonly IProjectRepository _projectRepository;

    public GetProjectsEndpoint(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var projects = await _projectRepository.GetAllAsync(ct);

        await SendOkAsync(projects.ToContract(), ct);
    }
}