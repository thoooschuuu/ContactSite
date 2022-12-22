using FastEndpoints;
using Microsoft.Extensions.Options;
using TSITSolutions.ContactSite.Server.Caching;
using TSITSolutions.ContactSite.Server.Core.Services;
using TSITSolutions.ContactSite.Server.Mapping;
using TSITSolutions.ContactSite.Server.Projects.Contracts;

namespace TSITSolutions.ContactSite.Server.Projects;

public class GetProjectEndpoint : Endpoint<GetProjectRequest>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IOptionsMonitor<CachingSettings> _cachingSettingsMonitor;

    public GetProjectEndpoint(IProjectRepository projectRepository, IOptionsMonitor<CachingSettings> cachingSettings)
    {
        _projectRepository = projectRepository;
        _cachingSettingsMonitor = cachingSettings;
    }
    
    public override void Configure()
    {
        Get("/api/projects/{id:guid}");
        AllowAnonymous();
        if(_cachingSettingsMonitor.CurrentValue.Enabled)
        {
            ResponseCache(_cachingSettingsMonitor.CurrentValue.Duration);
        }
    }

    public override async Task HandleAsync(GetProjectRequest request, CancellationToken ct)
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