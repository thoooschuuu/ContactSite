namespace TSITSolutions.ContactSite.Server.Projects.Contracts;

public class ProjectsResponse
{
    public IEnumerable<ProjectResponse> Projects { get; set; } = Enumerable.Empty<ProjectResponse>();
}