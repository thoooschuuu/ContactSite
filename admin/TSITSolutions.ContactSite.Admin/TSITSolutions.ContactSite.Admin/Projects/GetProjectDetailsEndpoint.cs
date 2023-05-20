using FastEndpoints;
using TSITSolutions.ContactSite.Admin.Core.Model;
using TSITSolutions.ContactSite.Admin.Core.Services;
using TSITSolutions.ContactSite.Admin.Projects.Contracts;
using TSITSolutions.ContactSite.Admin.Projects.Mapper;

namespace TSITSolutions.ContactSite.Admin.Projects;

public class GetProjectDetailsEndpoint : Endpoint<GetProjectDetailsRequest, ProjectDetailsResponse, GetProjectDetailsMapper>
{
    private readonly IProjectRepository _projectRepository;

    public GetProjectDetailsEndpoint(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }
    
    public override void Configure()
    {
        Get("/projects/{id:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetProjectDetailsRequest req, CancellationToken ct)
    {
        var project = await _projectRepository.GetByIdAsync(req.Id, ct);
        if (project == Project.Empty)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        Response = Map.FromEntity(project);
        await SendAsync(Response, cancellation: ct);
    }
}