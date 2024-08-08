using Autodissmark.Domain.ApplicationModels;

namespace Autodissmark.ApplicationDataAccess.Repositories.ReadRepositories.Contracts;

public interface IAcapellaReadRepository
{
    Task<AcapellaModel> GetById(int id, CancellationToken ct = default);
    Task<ICollection<AcapellaModel>> GetAllByTextId(int textId, CancellationToken ct = default);
    Task<ICollection<AcapellaModel>> GetPageByTextId(int textId, int pageSize, int pageNumber, CancellationToken ct = default);
    Task<int> GetAuthorId(int id, CancellationToken ct = default);
}
