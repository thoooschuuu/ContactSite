using FastEndpoints;
using TSITSolutions.ContactSite.Admin.Core.Services;
using TSITSolutions.ContactSite.Admin.Projects.Contracts;
using TSITSolutions.ContactSite.Admin.Projects.Mapper;

namespace TSITSolutions.ContactSite.Admin.Projects;

public class AddProjectEndpoint : Endpoint<AddProjectRequest, ProjectDetailsResponse, AddProjectRequestMapper>
{
    private readonly IProjectRepository _projectRepository;

    public AddProjectEndpoint(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public override void Configure()
    {
        Post("/projects");
        AllowAnonymous();
    }

    public override async Task HandleAsync(AddProjectRequest projectRequest, CancellationToken ct)
    {
        var project = Map.ToEntity(projectRequest);
        await _projectRepository.AddAsync(project, ct);
        Response = Map.FromEntity(project);
        await SendAsync(Response, cancellation: ct);
    }
}