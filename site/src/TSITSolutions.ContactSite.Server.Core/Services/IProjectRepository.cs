namespace TSITSolutions.ContactSite.Server.Core.Services;

public interface IProjectRepository
{
    ValueTask<IEnumerable<Project>> GetAllAsync(string? culture = null, CancellationToken ct = default);
    ValueTask<Project> GetByIdAsync(Guid id, string? culture = null, CancellationToken ct = default);
}