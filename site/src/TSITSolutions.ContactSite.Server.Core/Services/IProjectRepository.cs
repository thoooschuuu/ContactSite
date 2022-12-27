namespace TSITSolutions.ContactSite.Server.Core.Services;

public interface IProjectRepository
{
    ValueTask<IEnumerable<Project>> GetAllAsync(string? language = null, CancellationToken ct = default);
    ValueTask<Project> GetByIdAsync(Guid id, string? language = null, CancellationToken ct = default);
}