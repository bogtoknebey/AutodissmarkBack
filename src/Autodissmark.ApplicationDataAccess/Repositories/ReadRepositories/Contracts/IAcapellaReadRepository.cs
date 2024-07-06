using Autodissmark.Domain.ApplicationModels;

namespace Autodissmark.ApplicationDataAccess.Repositories.ReadRepositories.Contracts;

public interface IAcapellaReadRepository
{
    Task<AcapellaModel> GetById(int id, CancellationToken ct = default);
    Task<ICollection<AcapellaModel>> GetByTextId(int textId, CancellationToken ct = default);
    Task<int> GetAuthorId(int id, CancellationToken ct = default);
}
