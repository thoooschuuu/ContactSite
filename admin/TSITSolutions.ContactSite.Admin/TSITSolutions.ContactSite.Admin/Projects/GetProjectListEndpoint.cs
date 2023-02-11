using FastEndpoints;
using TSITSolutions.ContactSite.Admin.Core.Services;
using TSITSolutions.ContactSite.Admin.Projects.Contracts;
using TSITSolutions.ContactSite.Admin.Projects.Mapper;

namespace TSITSolutions.ContactSite.Admin.Projects;

public class GetProjectListEndpoint : EndpointWithoutRequest<ProjectsResponse, GetProjectListMapper>
{
    private readonly IProjectRepository _projectRepository;

    public GetProjectListEndpoint(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }
    
    public override void Configure()
    {
        Get("/projects");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var projects = await  _projectRepository.GetAllAsync(ct);
        Response = Map.FromEntity(projects);
        await SendAsync(Response, cancellation: ct);
    }
}