using Autodissmark.Domain.ApplicationModels;

namespace Autodissmark.ApplicationDataAccess.Repositories.WriteRepositories.Contracts;

public interface IAcapellaWriteRepository
{
    Task<int> Create(AcapellaModel model, CancellationToken ct = default);
    Task Delete(int id, CancellationToken ct = default);
}
