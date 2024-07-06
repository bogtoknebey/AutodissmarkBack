using Autodissmark.Domain.ApplicationModels.Diss;

namespace Autodissmark.ApplicationDataAccess.Repositories.WriteRepositories.Contracts;

public interface IDissWriteRepository
{
    Task<int> Create(DissModel model, CancellationToken ct = default);
    Task Delete(int id, CancellationToken ct = default);
}
