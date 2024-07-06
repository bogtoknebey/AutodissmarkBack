using Autodissmark.Domain.ApplicationModels;

namespace Autodissmark.ApplicationDataAccess.Repositories.ReadRepositories.Contracts;

public interface IBeatReadRepository
{
    Task<BeatModel> GetById(int id, CancellationToken ct = default);
}
