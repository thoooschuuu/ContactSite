namespace TSITSolutions.ContactSite.Admin.Projects.Contracts;

public class ProjectsResponse
{
    public IEnumerable<ProjectListResponse> Projects { get; set; } = Enumerable.Empty<ProjectListResponse>();
}