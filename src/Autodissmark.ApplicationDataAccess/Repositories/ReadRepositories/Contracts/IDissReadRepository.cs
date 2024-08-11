using Autodissmark.Domain.ApplicationModels.Diss;

namespace Autodissmark.ApplicationDataAccess.Repositories.ReadRepositories.Contracts;

public interface IDissReadRepository
{
    Task<DissModel> GetById(int id, CancellationToken ct = default);
    Task<ICollection<DissModel>> GetAllByTextId(int textId, CancellationToken ct = default);
    Task<ICollection<DissModel>> GetPageByTextId(int textId, int pageSize, int pageNumber, CancellationToken ct = default);
    Task<int> GetAuthorId(int id, CancellationToken ct = default);
}
