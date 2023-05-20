using TSITSolutions.ContactSite.Admin.Core.Model;

namespace TSITSolutions.ContactSite.Admin.Core.Services;

public interface IProjectRepository
{
    ValueTask<IEnumerable<Project>> GetAllAsync(CancellationToken ct = default);
    ValueTask<Project> GetByIdAsync(Guid id, CancellationToken ct = default);
    ValueTask AddAsync(Project project, CancellationToken ct);
}