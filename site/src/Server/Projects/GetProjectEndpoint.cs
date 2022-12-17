using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using TSITSolutions.ContactSite.Server.Core.Services;
using TSITSolutions.ContactSite.Server.Mapping;
using TSITSolutions.ContactSite.Server.Projects.Contracts;

namespace TSITSolutions.ContactSite.Server.Projects;

[HttpGet("/api/projects/{id:guid}")]
[AllowAnonymous]
public class GetProjectEndpoint : Endpoint<GetProjectRequest>
{
    private readonly IProjectRepository _projectRepository;

    public GetProjectEndpoint(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public override async Task HandleAsync(GetProjectRequest request, CancellationToken ct = default)
    {
        var project = await _projectRepository.GetByIdAsync(request.Id, ct);

        if (project is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }
        await SendOkAsync(project.ToContract(), ct);
    }
}