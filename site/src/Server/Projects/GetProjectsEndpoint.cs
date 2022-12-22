using FastEndpoints;
using Microsoft.Extensions.Options;
using TSITSolutions.ContactSite.Server.Caching;
using TSITSolutions.ContactSite.Server.Core.Services;
using TSITSolutions.ContactSite.Server.Mapping;
using TSITSolutions.ContactSite.Shared.Projects;

namespace TSITSolutions.ContactSite.Server.Projects;

public class GetProjectsEndpoint : EndpointWithoutRequest<ProjectsResponse>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IOptionsMonitor<CachingSettings> _cachingSettingsMonitor;

    public GetProjectsEndpoint(IProjectRepository projectRepository, IOptionsMonitor<CachingSettings> cachingSettings)
    {
        _projectRepository = projectRepository;
        _cachingSettingsMonitor = cachingSettings;
    }

    public override void Configure()
    {
        Get("/api/projects");
        AllowAnonymous();
        if(_cachingSettingsMonitor.CurrentValue.Enabled)
        {
            ResponseCache(_cachingSettingsMonitor.CurrentValue.Duration);
        }
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var projects = await _projectRepository.GetAllAsync(ct);

        await SendOkAsync(projects.ToContract(), ct);
    }
}