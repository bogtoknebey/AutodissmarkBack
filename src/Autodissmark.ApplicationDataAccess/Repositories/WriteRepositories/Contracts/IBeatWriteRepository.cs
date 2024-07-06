using Autodissmark.Domain.ApplicationModels;

namespace Autodissmark.ApplicationDataAccess.Repositories.WriteRepositories.Contracts;

public interface IBeatWriteRepository
{
    Task<int> Create(BeatModel model, CancellationToken ct = default);
}


