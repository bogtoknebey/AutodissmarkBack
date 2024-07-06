using Autodissmark.Domain.ApplicationModels.Diss;

namespace Autodissmark.ApplicationDataAccess.Repositories.ReadRepositories.Contracts;

public interface IDissReadRepository
{
    Task<DissModel> GetById(int id, CancellationToken ct = default);
    Task<int> GetAuthorId(int id, CancellationToken ct = default);
}
