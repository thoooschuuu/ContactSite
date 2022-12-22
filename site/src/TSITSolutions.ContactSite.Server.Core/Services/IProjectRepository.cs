namespace TSITSolutions.ContactSite.Server.Core.Services;

public interface IProjectRepository
{
    ValueTask<IEnumerable<Project>> GetAllAsync(CancellationToken ct = default);
    ValueTask<Project?> GetByIdAsync(Guid id, CancellationToken ct = default);
}